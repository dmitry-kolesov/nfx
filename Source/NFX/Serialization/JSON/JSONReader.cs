/*<FILE_LICENSE>
* NFX (.NET Framework Extension) Unistack Library
* Copyright 2003-2014 IT Adapter Inc / 2015 Aum Code LLC
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
</FILE_LICENSE>*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NFX.CodeAnalysis;
using NFX.CodeAnalysis.Source;
using NFX.CodeAnalysis.JSON;
using NFX.DataAccess.CRUD;

namespace NFX.Serialization.JSON
{
    /// <summary>
    /// Provides deserialization functionality from JSON format
    /// </summary>
    public static class JSONReader 
    {
        
        #region Public
      
            public static dynamic DeserializeDynamic(Stream stream, Encoding encoding = null)
            {
               return deserializeDynamic( read(stream, encoding));
            }

            public static dynamic DeserializeDynamic(string source)
            {
               return deserializeDynamic( read(source));
            }

            public static dynamic DeserializeDynamic(ISourceText source)
            {
               return deserializeDynamic( read(source));
            }

            public static IJSONDataObject DeserializeDataObject(Stream stream, Encoding encoding = null)
            {
               return deserializeObject( read(stream, encoding));
            }

            public static IJSONDataObject DeserializeDataObject(string source)
            {
               return deserializeObject( read(source));
            }

            public static IJSONDataObject DeserializeDataObject(ISourceText source)
            {
               return deserializeObject( read(source));
            }


            //Future??? does not make sense to deserialize in CLR types
            //public static TResult Deserialize<TResult>(Stream stream) where TResult : new()
            //{
            //    var data = read(stream);
            //    return default(TResult);
            //}

            //public TResult Deserialize<TResult>(string source) where TResult : new()
            //{
            //    var data = read(source);
            //    return default(TResult);
            //}

            /// <summary>
            /// Converts JSONMap into typed row of the requested type. 
            /// The requested type must be derived from NFX.DataAccess.CRUD.TypedRow.
            /// The extra data found in JSON map will be placed in AmorphousData dictionary if the row implemets IAmorphousData, discarded otherwise.
            /// Note: This method provides "the best match" and does not guarantee that all data will/can be converted from JSON, i.e.
            ///  it can only convert one dimensional arrays and Lists of either primitive or TypeRow-derived entries
            /// </summary>
            /// <param name="type">TypedRow subtype to convert into</param>
            /// <param name="jsonMap">JSON data to convert into row</param>
            /// <param name="fromUI">When true indicates that data came from UI, hence NonUI-marked fields should be skipped. True by default</param>
            public static TypedRow ToRow(Type type, JSONDataMap jsonMap, bool fromUI = true)
            {
              if (!typeof(TypedRow).IsAssignableFrom(type) || jsonMap==null) 
               throw new JSONDeserializationException(StringConsts.ARGUMENT_ERROR+"JSONDynamicObject.ToRow(type|jsonMap=null)");
              var field = "";
              try
              {
                return toTypedRow(type, jsonMap, ref field, fromUI);
              }
              catch(Exception error)
              {
                throw new JSONDeserializationException("JSONReader.ToRow(jsonMap) error in '{0}': {1}".Args(field, error.ToMessageWithType()), error);
              }
            }

            /// <summary>
            /// Generic version of 
            /// <see cref="ToRow(Type, JSONDataMap, bool)"/>
            /// </summary>
            /// <typeparam name="T">TypedRow</typeparam>
            public static T ToRow<T>(JSONDataMap jsonMap, bool fromUI = true) where T: TypedRow
            {
              return ToRow(typeof(T), jsonMap, fromUI) as T;
            }


            /// <summary>
            /// Converts JSONMap into supplied row instance. 
            /// The extra data found in JSON map will be placed in AmorphousData dictionary if the row implemets IAmorphousData, discarded otherwise.
            /// Note: This method provides "the best match" and does not guarantee that all data will/can be converted from JSON, i.e.
            ///  it can only convert one dimensional arrays and Lists of either primitive or TypeRow-derived entries
            /// </summary>
            /// <param name="row">Row instance to convert into</param>
            /// <param name="jsonMap">JSON data to convert into row</param>
            /// <param name="fromUI">When true indicates that data came from UI, hence NonUI-marked fields should be skipped. True by default</param>
            public static void ToRow(Row row, JSONDataMap jsonMap, bool fromUI = true)
            {
              if (row == null || jsonMap == null)
                throw new JSONDeserializationException(StringConsts.ARGUMENT_ERROR + "JSONDynamicObject.ToRow(row|jsonMap=null)");
              var field = "";
              try
              {
                toRow(row, jsonMap, ref field, fromUI);
              }
              catch (Exception error)
              {
                throw new JSONDeserializationException("JSONReader.ToRow(row, jsonMap) error in '{0}': {1}".Args(field, error.ToMessageWithType()), error);
              }
            }




            private static TypedRow toTypedRow(Type type, JSONDataMap jsonMap, ref string field, bool fromUI)
            { 
              var row = (TypedRow)Activator.CreateInstance(type);
              toRow(row, jsonMap, ref field, fromUI);
              return row;
            }

            private static void toRow(Row row, JSONDataMap jsonMap, ref string field, bool fromUI)
            {
              var amorph = row as IAmorphousData;
              foreach(var mfld in jsonMap)
              {
                    var fv = mfld.Value;

                    var rfd = row.Schema[mfld.Key];
                    field = mfld.Key;

                    if (rfd==null)
                    {
                        if (amorph!=null)
                        {
                          if (amorph.AmorphousDataEnabled)
                            amorph.AmorphousData[mfld.Key] = fv;
                        }
                        continue;
                    }

                    if (fromUI && rfd.NonUI) continue;//skip NonUI fields
                
                    if (fv==null) 
                          row.SetFieldValue(rfd, null);
                    else    
                    if (fv is JSONDataMap)
                    {
                          if (typeof(TypedRow).IsAssignableFrom(rfd.Type))
                           row.SetFieldValue(rfd, ToRow(rfd.Type, (JSONDataMap)fv));
                          else
                           row.SetFieldValue(rfd, fv);//try to set row's field to MAP directly
                    }
                    else if (rfd.NonNullableType==typeof(TimeSpan) && (fv is ulong || fv is long))
                         row.SetFieldValue(rfd, TimeSpan.FromTicks(fv is ulong ? (long)(ulong)fv : (long)fv)); 
                    else if (fv is int || fv is long || fv is ulong || fv is double || fv is bool)
                          row.SetFieldValue(rfd, fv);
                    else if (fv is byte[] && rfd.Type==typeof(byte[]))//optimization byte array assignment without copies
                    {
                          var passed = (byte[])fv;
                          var arr = new byte[passed.Length];
                          Array.Copy(passed, arr, passed.Length);
                          row.SetFieldValue(rfd, arr);
                    }
                    else if (fv is JSONDataArray || fv.GetType().IsArray)
                    {
                          JSONDataArray arr;
                          if (fv is JSONDataArray)  arr = (JSONDataArray)fv;
                          else
                          {
                              arr = new JSONDataArray(((Array)fv).Length);
                              foreach(var elm in (System.Collections.IEnumerable)fv) arr.Add(elm);
                          }
                  
                          if (rfd.Type.IsArray)
                          {
                                var raet = rfd.Type.GetElementType();//row array element type 
                                if (typeof(TypedRow).IsAssignableFrom(raet))
                                {
                                  var narr = Array.CreateInstance(raet, arr.Count);
                                  for(var i=0; i<narr.Length; i++)
                                    narr.SetValue( ToRow(raet, arr[i] as JSONDataMap), i);
                                  row.SetFieldValue(rfd, narr);   
                                }//else primitives
                                else
                                {
                                  var narr = Array.CreateInstance(raet, arr.Count);
                                  for(var i=0; i<narr.Length; i++)
                                    if (arr[i]!=null)
                                      narr.SetValue( StringValueConversion.AsType(arr[i].ToString(), raet, false), i);

                                  row.SetFieldValue(rfd, narr);
                                }
                          }
                          else//List
                          {
                                var gargs = rfd.Type.GetGenericArguments();
                                if (gargs!=null && gargs.Length==1)
                                {
                                  var lst = Activator.CreateInstance(rfd.Type) as System.Collections.IList;
                                  if (gargs[0].IsPrimitive)
                                  {
                                    for(var i=0; i<arr.Count; i++)
                                      if (arr[i]!=null)
                                        lst.Add( StringValueConversion.AsType(arr[i].ToString(), gargs[0], false) );
                                  }
                                  else
                                  {
                                    for(var i=0; i<arr.Count; i++)
                                      if (arr[i] is JSONDataMap)
                                        lst.Add( ToRow(gargs[0], arr[i] as JSONDataMap) );
                                  }
                                  row.SetFieldValue(rfd, lst);
                                }
                          }
                    }
                    else
                    {
                          //Try to get String containing JSON
                          if (fv is string)
                          {
                            var sfv = (string)fv;
                            if (rfd.Type==typeof(string)) 
                            {
                              row.SetFieldValue(rfd, sfv);
                              continue;
                            }
                    
                            if (typeof(TypedRow).IsAssignableFrom(rfd.Type))
                            {
                             if (sfv.IsNotNullOrWhiteSpace())
                                row.SetFieldValue(rfd, ToRow(rfd.Type, (JSONDataMap)deserializeObject( read(sfv))));
                             continue;
                            }
                            if (typeof(IJSONDataObject).IsAssignableFrom(rfd.Type))
                            {
                             if (sfv.IsNotNullOrWhiteSpace())
                                row.SetFieldValue(rfd, deserializeObject( read(sfv)));//try to set row's field to MAP directly
                             continue;
                            }
                          }

                          row.SetFieldValue(rfd, StringValueConversion.AsType(fv.ToString(), rfd.Type, false));//<--Type conversion
                    }
              }//foreach

              //20140914 DKh
              var form = row as FormModel;
              if (form != null)
              {
                form.FormMode = jsonMap[FormModel.JSON_MODE_PROPERTY].AsEnum<FormMode>(FormMode.Unspecified);
                form.CSRFToken = jsonMap[FormModel.JSON_CSRF_PROPERTY].AsString();
              }

              if (amorph!=null && amorph.AmorphousDataEnabled )
                amorph.AfterLoad("json");
            } 

            
        #endregion

        #region .pvt .impl
            
            private static dynamic deserializeDynamic(object root)
            {
                var data = deserializeObject(root);
                if (data == null) return null;
                
                return new JSONDynamicObject( data );
            }

            private static IJSONDataObject deserializeObject(object root)
            {
                if (root==null) return null;

                var data = root as IJSONDataObject;
                
                if (data == null)
                  data = new JSONDataMap{{"value", root}};
                
                return data;
            }
            
            private static object read(Stream stream, Encoding encoding)
            {
                using(var source = encoding==null ? new StreamSource(stream, JSONLanguage.Instance)
                                                  : new StreamSource(stream, encoding, JSONLanguage.Instance))
                    return read(source);
            }

            private static object read(string data)
            {
                var source = new StringSource(data, JSONLanguage.Instance);
                return read(source);
            }

            private static object read(ISourceText source)
            {
                var lexer = new JSONLexer(source, throwErrors: true);
                var parser = new JSONParser(lexer, throwErrors: true);
                
                parser.Parse();
                
                return parser.ResultContext.ResultObject;   
            }
        #endregion


            
    }
}
