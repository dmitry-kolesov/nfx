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
using System.Reflection;

namespace NFX.Serialization.Slim
{
    /// <summary>
    /// Provides a registry of types, types that do not need to be described in a serialization stream
    /// </summary>
    public class TypeRegistry : IEnumerable<Type>
    {
       #region COSNSTS
           /// <summary>
           /// Denotes a special type which is object==null
           /// </summary>
           public const string NULL_HANDLE="$N";
       
       #endregion

       #region STATIC

           public static bool IsNullHandle(string handle)
           {
             if (handle==null) return true;
             var l = handle.Length;
             if (l==0) return true;
             return l==2 && handle[0]=='$' && handle[1]=='N';
             
             //20140701 DKh speed improvement
             //string.Equals(handle, NULL_HANDLE, StringComparison.OrdinalIgnoreCase);
           }


           /// <summary>
           /// Returns Record-model specific types
           /// </summary>
           public static IEnumerable<Type> RecordModelTypes
           {
             get
             {
               var asm = Assembly.GetAssembly(typeof(RecordModel.Record));
               var types = asm.GetTypes().Where(t => t.Namespace == "NFX.RecordModel" && Attribute.IsDefined(t, typeof(SerializableAttribute))).ToList();
               types.Add(typeof(List<RecordModel.ModelBase>));
               types.Add(typeof(RecordModel.ModelBase[]));
               types.Add(typeof(List<RecordModel.Notification>));
               types.Add(typeof(RecordModel.Notification[]));
               types.Add(typeof(List<string>));
               types.Add(typeof(Exception));

               return types;
             }
           }


           /// <summary>
           /// Returns Glue protocol specific types
           /// </summary>
           public static IEnumerable<Type> GlueProtocolTypes
           {
             get
             {
               yield return typeof(Glue.Protocol.Msg);
               yield return typeof(Glue.Protocol.RemoteExceptionData);
               yield return typeof(Glue.Protocol.RequestMsg);
               yield return typeof(Glue.Protocol.RequestAnyMsg);
               yield return typeof(Glue.Protocol.ResponseMsg);
               yield return typeof(Glue.Protocol.Header);
               yield return typeof(Glue.Protocol.Header []);
               yield return typeof(Glue.Protocol.Headers);
               yield return typeof(Glue.Protocol.AuthenticationHeader);
               yield return typeof(NFX.Security.IDPasswordCredentials);
               yield return typeof(NFX.Security.GDIDCredentials);
               yield return typeof(NFX.Security.SocialNetTokenCredentials);
               yield return typeof(Exception);
             }
           }


           /// <summary>
           /// Returns common types for DataAccess.CRUD
           /// </summary>
           public static IEnumerable<Type> DataAccessCRUDTypes
           {
             get
             {
               yield return typeof(DataAccess.IDataStoreKey);
               yield return typeof(DataAccess.CRUD.TableAttribute);
               yield return typeof(DataAccess.CRUD.FieldAttribute);
               yield return typeof(DataAccess.CRUD.FieldAttribute[]);
               yield return typeof(List<DataAccess.CRUD.FieldAttribute>);
               yield return typeof(DataAccess.CRUD.Schema);
               yield return typeof(DataAccess.CRUD.Schema.FieldDef);
               yield return typeof(OrderedRegistry<DataAccess.CRUD.Schema.FieldDef>);
               yield return typeof(RegistryDictionary<DataAccess.CRUD.Schema.FieldDef>);
               yield return typeof(List<DataAccess.CRUD.Schema.FieldDef>);
               yield return typeof(DataAccess.CRUD.RowChange);
               yield return typeof(DataAccess.CRUD.RowChange[]);
               yield return typeof(List<DataAccess.CRUD.RowChange>);

               yield return typeof(DataAccess.CRUD.Row);
               yield return typeof(DataAccess.CRUD.Row[]);
               yield return typeof(List<DataAccess.CRUD.Row>);
               
               yield return typeof(DataAccess.CRUD.DynamicRow);
               yield return typeof(DataAccess.CRUD.TypedRow);
               yield return typeof(DataAccess.CRUD.Query);
               yield return typeof(DataAccess.CRUD.Query.Param);
               yield return typeof(DataAccess.CRUD.Query.Param[]);
               yield return typeof(DataAccess.CRUD.Rowset);
               yield return typeof(DataAccess.CRUD.Table);
               yield return typeof(DataAccess.CRUD.FormModel);

               yield return typeof(DataAccess.Distributed.Command);
               yield return typeof(DataAccess.Distributed.Command.Param);
               yield return typeof(DataAccess.Distributed.Command.Param[]);
               yield return typeof(DataAccess.Distributed.Parcel);
               yield return typeof(DataAccess.Distributed.Parcel[]);
               yield return typeof(DataAccess.Distributed.IReplicationVersionInfo);
               
             }
           }

           /// <summary>
           /// Returns frequently-used generic collections
           /// </summary>
           public static IEnumerable<Type> CommonCollectionTypes
           {
             get
             {
               yield return typeof(List<object>);
               yield return typeof(List<string>);
               yield return typeof(Dictionary<object, object>);
               yield return typeof(KeyValuePair<object, object>);
               yield return typeof(KeyValuePair<object, object>[]);
               yield return typeof(Dictionary<string, object>);
               yield return typeof(KeyValuePair<string, object>);
               yield return typeof(KeyValuePair<string, object>[]);

               yield return typeof(IEqualityComparer<object>);
               yield return typeof(IEqualityComparer<string>);

               yield return typeof(NFX.Serialization.JSON.JSONDataArray);
               yield return typeof(NFX.Serialization.JSON.JSONDataMap);
               yield return typeof(NFX.Serialization.JSON.JSONDynamicObject);
             }
           }

           /// <summary>
           /// Returns common primitive types - use when much boxing is expected
           /// </summary>
           public static IEnumerable<Type> BoxedCommonTypes
           {
             get
             {
               yield return typeof(int);
               yield return typeof(uint);
               yield return typeof(byte);
               yield return typeof(sbyte);
               yield return typeof(long);
               yield return typeof(ulong);
               yield return typeof(short);
               yield return typeof(ushort);
               yield return typeof(float);
               yield return typeof(double);
               yield return typeof(decimal);
               yield return typeof(char);
               yield return typeof(bool);
               yield return typeof(DateTime);
               yield return typeof(TimeSpan);
               yield return typeof(NFX.DataAccess.Distributed.GDID);
               yield return typeof(NFX.FID);
             }
           }

           /// <summary>
           /// Returns common nullable types - use when much boxing is expected
           /// </summary>
           public static IEnumerable<Type> BoxedCommonNullableTypes
           {
             get
             {
               yield return typeof(int?);
               yield return typeof(uint?);
               yield return typeof(byte?);
               yield return typeof(sbyte?);
               yield return typeof(long?);
               yield return typeof(ulong?);
               yield return typeof(short?);
               yield return typeof(ushort?);
               yield return typeof(float?);
               yield return typeof(double?);
               yield return typeof(decimal?);
               yield return typeof(char?);
               yield return typeof(bool?);
               yield return typeof(DateTime?);
               yield return typeof(TimeSpan?);
               yield return typeof(NFX.DataAccess.Distributed.GDID?);
               yield return typeof(NFX.FID?);
             }
           }


           /// <summary>
           /// Returns PortableObjectDocument types
           /// </summary>
           public static IEnumerable<Type> PODTypes
           {
             get
             {
               foreach( var t in BoxedCommonTypes) yield return t;
               foreach( var t in BoxedCommonNullableTypes) yield return t;
               yield return typeof(POD.PortableObjectDocument);
               yield return typeof(POD.MetaType);
               yield return typeof(POD.MetaPrimitiveType);
               yield return typeof(POD.MetaComplexType);
               yield return typeof(POD.MetaComplexType.MetaField);
               yield return typeof(POD.CompositeData);
               yield return typeof(POD.CompositeCustomData);
               yield return typeof(POD.CompositeReflectedData);
               yield return typeof(POD.CustomTypedEntry);
               yield return typeof(Environment.BuildInformation);
               yield return typeof(List<POD.MetaType>);
               yield return typeof(POD.MetaType[]);
               yield return typeof(List<POD.CompositeData>);
               yield return typeof(POD.CompositeData[]);
               yield return typeof(List<POD.MetaComplexType.MetaField>);
               yield return typeof(POD.MetaComplexType.MetaField[]);
             }
           }


           //20140701 DKh - speed optimization
           private const int STR_HNDL_POOL_SIZE = 512;
           private readonly static string[] STR_HNDL_POOL;

           static TypeRegistry()
           {
              STR_HNDL_POOL = new string[STR_HNDL_POOL_SIZE];
              for(var i=0; i<STR_HNDL_POOL_SIZE; i++)
               STR_HNDL_POOL[i] = '$'+i.ToString();
           }
           //20140701 DKh - speed optimization

       #endregion

       #region .ctors
          
           public TypeRegistry()
           {  //WARNING!!! These types MUST be at the following positions always at the pre-defined index:
              add(typeof(object));//must be at index zero
              add(typeof(object[]));//must be at index 1
              add(typeof(byte[]));
           }
          
           /// <summary>
           /// Initializes TypeRegistry with types from other sources
           /// </summary>
           public TypeRegistry(params IEnumerable<Type>[] others) : this()
           {
             if (others!=null)
              foreach(var other in others)
               if (other!=null)
                foreach(var t in other) Add(t);
           }

          

       #endregion

       #region Fields
           private Dictionary<Type, int> m_Types =  new Dictionary<Type, int>(0xff);
           private List<Type> m_List = new List<Type>(0xff);

           private ulong m_CSum;

       #endregion


       #region Properties
           
           /// <summary>
           /// How many items in the registry
           /// </summary>
           public int Count { get{ return m_List.Count;}}

           /// <summary>
           /// Returns quick checksum of type registry contents.
           /// It is updated when new types get added into the registry
           /// </summary>
           public ulong CSum{  get{ return m_CSum;}}
           


                      private static Dictionary<string, Type> s_Types = new Dictionary<string,Type>(StringComparer.Ordinal);

           /// <summary>
           /// Returns type by handle i.e. '$11' or full name. Throws in case of error
           /// </summary>
           public Type this[string handle]
           {
            get
            {
              try
              {
                if (IsNullHandle(handle)) return typeof(object);
            
                if (handle[0]=='$')
                {
                   // var idx = int.Parse(handle.Substring(1));
                    //20140701 DKh speed improvement
                    var idx = quickParseInt(handle);
                    if (idx<m_List.Count)
                     return m_List[idx];
                    throw new Exception();
                }

                Type result;
                if (!s_Types.TryGetValue(handle, out result))
                {
                  result = Type.GetType(handle, true);
                  var dict = new Dictionary<string,Type>(s_Types, StringComparer.Ordinal);
                  dict[handle] = result;
                  s_Types = dict;//atomic
                }

                bool added;
                GetTypeIndex(result, out added);
                return result;
              }
              catch
              {
                throw new SlimInvalidTypeHandleException("TypeRegistry[handle] is invalid: " + (handle ?? StringConsts.NULL_STRING));
              }
            }
           }

                   private int quickParseInt(string str)
                   {
                      int result = 0;
                      var l = str.Length;
                      for(var i=1; i<l; i++) //0=$, starts at index 1
                      {
                        var d = str[i] - '0';
                        if (d<0 || d>9) throw new SlimException("Invalid type handle int: "+str);
                        result *= 10;
                        result += d;
                      }

                      return result; 
                   }

       #endregion

       #region Public
               /// <summary>
               /// See this[handle]
               /// </summary>
               public Type GetByHandle(string handle)
               {
                 return this[handle];
               }


               /// <summary>
               /// Adds the type if it not already in registry and returns true
               /// </summary>
               public bool Add(Type type)
               {
                 var idx = 0;
                 if (m_Types.TryGetValue(type, out idx)) return false;
                 add(type);
                 return true;
               }

               public int GetTypeIndex(Type type, out bool added)
               {

                 added = false;
                 var idx = 0;
                 if (m_Types.TryGetValue(type, out idx)) return idx;

                 added = true;
                 idx = add(type);

                 return idx;
               }

               /// <summary>
               /// Returns type index formatted as handle if type exists in registry, or fully qualified type name otherwise
               /// </summary>
               public string GetTypeHandle(Type type)
               {
                 bool added;
                 var idx = GetTypeIndex(type, out added);
                 if (!added)
                 {
                   if (idx<STR_HNDL_POOL_SIZE) return STR_HNDL_POOL[idx];
                   return '$'+idx.ToString();
                 }

                 return type.AssemblyQualifiedName;
               }



               public IEnumerator<Type> GetEnumerator()
               {
                   return m_List.GetEnumerator();
               }

               System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
               {
                   return m_List.GetEnumerator();
               }


       #endregion

       #region .pvt .impl

           private int add(Type t)
           {
             m_List.Add(t);
             var idx = m_List.Count - 1;
             m_Types.Add(t, idx);

             var tn = t.FullName;
             var len = tn.Length;
             int csum =   (  ((byte)tn[0])     << 16 )  | 
                          (  ((byte)tn[len-1]) << 8  )  | 
                          (   len & 0xff             );

             m_CSum += (ulong)csum; //unchecked is not needed as there is never going to be> 4,000,000,000 types in registry
             return idx;
           }



       #endregion


    }

}
