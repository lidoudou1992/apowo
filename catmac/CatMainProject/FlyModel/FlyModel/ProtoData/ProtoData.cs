using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using ProtoBuf;

namespace FlyModel.Proto
{
    public abstract class ProtoObject : IProtoObject
    {
         public abstract void WriteTo(Stream stream);
         public abstract void ReadFrom(Stream stream);
         public abstract void ReadFrom(byte[] bytes,int index,int count);
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 意见信息
    /// </summary>
    public partial class AdviseData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Content"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<AdviseData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private string _content	;
        public string Content	
        {
           get { return _content	;}
           set
           { 
               _content	 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Content	 = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               if(Content	 != null)
               {
                   ProtoWriter.WriteFieldHeader(1, WireType.String, writer);
                   ProtoWriter.WriteString(Content	, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if(Content	 != null)
            {
                sb.Append("Content	:" + Content	.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 账号信息
    /// </summary>
    public partial class AccountData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Name"] = 1,
            ["Password"] = 2,
            ["Device"] = 3,
            ["DeviceNumber"] = 4,
            ["Platform"] = 5,
            ["LoginType"] = 6,
            ["CheckTime"] = 7,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<AccountData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private string _name		;
        public string Name		
        {
           get { return _name		;}
           set
           { 
               _name		 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private string _password	;
        public string Password	
        {
           get { return _password	;}
           set
           { 
               _password	 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private string _device;
        /// <summary>
        /// 设备
        /// </summary>
        public string Device
        {
           get { return _device;}
           set
           { 
               _device = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private string _devicenumber;
        /// <summary>
        /// 设备号
        /// </summary>
        public string DeviceNumber
        {
           get { return _devicenumber;}
           set
           { 
               _devicenumber = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private string _platform;
        /// <summary>
        /// 平台
        /// </summary>
        public string Platform
        {
           get { return _platform;}
           set
           { 
               _platform = value;
               if(!memberSets.ContainsKey(5))
               { 
                    memberSets[5] = 1;
               } 
               else
               { 
                   memberSets[5]++;
               }
           }
        }

        private int _logintype;
        /// <summary>
        /// 登录方式 0：密码登录 1：平台登录 2：注册并登录
        /// </summary>
        public int LoginType
        {
           get { return _logintype;}
           set
           { 
               _logintype = value;
               if(!memberSets.ContainsKey(6))
               { 
                    memberSets[6] = 1;
               } 
               else
               { 
                   memberSets[6]++;
               }
           }
        }

        private string _checktime;
        /// <summary>
        /// 时间戳
        /// </summary>
        public string CheckTime
        {
           get { return _checktime;}
           set
           { 
               _checktime = value;
               if(!memberSets.ContainsKey(7))
               { 
                    memberSets[7] = 1;
               } 
               else
               { 
                   memberSets[7]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Name		 = reader.ReadString();
                        break;
                    case 2:
                        Password	 = reader.ReadString();
                        break;
                    case 3:
                        Device = reader.ReadString();
                        break;
                    case 4:
                        DeviceNumber = reader.ReadString();
                        break;
                    case 5:
                        Platform = reader.ReadString();
                        break;
                    case 6:
                        LoginType = reader.ReadInt32();
                        break;
                    case 7:
                        CheckTime = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               if(Name		 != null)
               {
                   ProtoWriter.WriteFieldHeader(1, WireType.String, writer);
                   ProtoWriter.WriteString(Name		, writer);
               }
           }
           if(memberSets.ContainsKey(2))
           {
               if(Password	 != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.String, writer);
                   ProtoWriter.WriteString(Password	, writer);
               }
           }
           if(memberSets.ContainsKey(3))
           {
               if(Device != null)
               {
                   ProtoWriter.WriteFieldHeader(3, WireType.String, writer);
                   ProtoWriter.WriteString(Device, writer);
               }
           }
           if(memberSets.ContainsKey(4))
           {
               if(DeviceNumber != null)
               {
                   ProtoWriter.WriteFieldHeader(4, WireType.String, writer);
                   ProtoWriter.WriteString(DeviceNumber, writer);
               }
           }
           if(memberSets.ContainsKey(5))
           {
               if(Platform != null)
               {
                   ProtoWriter.WriteFieldHeader(5, WireType.String, writer);
                   ProtoWriter.WriteString(Platform, writer);
               }
           }
           if(memberSets.ContainsKey(6))
           {
               ProtoWriter.WriteFieldHeader(6, WireType.Variant, writer);
               ProtoWriter.WriteInt32(LoginType, writer);
           }
           if(memberSets.ContainsKey(7))
           {
               if(CheckTime != null)
               {
                   ProtoWriter.WriteFieldHeader(7, WireType.String, writer);
                   ProtoWriter.WriteString(CheckTime, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if(Name		 != null)
            {
                sb.Append("Name		:" + Name		.ToString());
                sb.Append(",");
            }
            if(Password	 != null)
            {
                sb.Append("Password	:" + Password	.ToString());
                sb.Append(",");
            }
            if(Device != null)
            {
                sb.Append("Device:" + Device.ToString());
                sb.Append(",");
            }
            if(DeviceNumber != null)
            {
                sb.Append("DeviceNumber:" + DeviceNumber.ToString());
                sb.Append(",");
            }
            if(Platform != null)
            {
                sb.Append("Platform:" + Platform.ToString());
                sb.Append(",");
            }
            sb.Append("LoginType:" + LoginType.ToString());
            sb.Append(",");
            if(CheckTime != null)
            {
                sb.Append("CheckTime:" + CheckTime.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 登录成功
    /// </summary>
    public partial class LoginData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Actors"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<LoginData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private List<CharacterData> _actors	;
        public List<CharacterData> Actors	
        {
           get
           {
               if(_actors	==null)
               {
                   _actors	 = new List<CharacterData>();
               }
               return _actors	;
           }
           private set { _actors	=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempActors	 = new CharacterData();
                        var _tokenActors	 = ProtoReader.StartSubItem(reader);
                        _tempActors	.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenActors	, reader);
                        Actors	.Add(_tempActors	);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(_actors	 != null && _actors	.Count > 0)
           {
              for(int i = 0; i < Actors	.Count; i++)
              {
                  var temp = Actors	[i];
                  ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenActors	 = ProtoWriter.StartSubItem(Actors	, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenActors	, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
           if(_actors	 != null && _actors	.Count > 0)
           {
              sb.Append("Actors	:[");
              for(int i = 0; i < Actors	.Count; i++)
              {
                  sb.Append(Actors	[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 所有需要客户端发送单个ID的命令都用该协议封装
    /// </summary>
    public partial class IDMessage : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<IDMessage, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class IDListMessage : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<IDListMessage, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private List<long> _id;
        public List<long> Id
        {
           get
           {
               if(_id==null)
               {
                   _id = new List<long>();
               }
               return _id;
           }
           private set { _id=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempId = reader.ReadInt64();
                        Id.Add(_tempId);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(_id != null && _id.Count > 0)
           {
              for(int i = 0; i < Id.Count; i++)
              {
                  var temp = Id[i];
                  ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
                  ProtoWriter.WriteInt64(temp, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
           if(_id != null && _id.Count > 0)
           {
              sb.Append("Id:[");
              for(int i = 0; i < Id.Count; i++)
              {
                  sb.Append(Id[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class IDCountMessage : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Count"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<IDCountMessage, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id		;
        public long Id		
        {
           get { return _id		;}
           set
           { 
               _id		 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _count	;
        public int Count	
        {
           get { return _count	;}
           set
           { 
               _count	 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id		 = reader.ReadInt64();
                        break;
                    case 2:
                        Count	 = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id		, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id		:" + Id		.ToString());
            sb.Append(",");
            sb.Append("Count	:" + Count	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class CountMessage : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Count"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<CountMessage, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private int _count;
        public int Count
        {
           get { return _count;}
           set
           { 
               _count = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Count = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Count:" + Count.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 售出方式
    /// </summary>
    public enum Currency
    {
        None	 = 0,
        Coin	 = 1,
        Dollar	 = 2,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 性别
    /// </summary>
    public enum Gender
    {
        无 = 0,
        男 = 1,
        女 = 2,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 创建角色
    /// </summary>
    public partial class CreateCharacter : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["NickName"] = 1,
            ["Gender"] = 2,
            ["Dollar"] = 4,
            ["Coin"] = 5,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<CreateCharacter, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private string _nickname	;
        public string NickName	
        {
           get { return _nickname	;}
           set
           { 
               _nickname	 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private Gender _gender		;
        public Gender Gender		
        {
           get { return _gender		;}
           set
           { 
               _gender		 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _dollar		;
        public int Dollar		
        {
           get { return _dollar		;}
           set
           { 
               _dollar		 = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private long _coin			;
        public long Coin			
        {
           get { return _coin			;}
           set
           { 
               _coin			 = value;
               if(!memberSets.ContainsKey(5))
               { 
                    memberSets[5] = 1;
               } 
               else
               { 
                   memberSets[5]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        NickName	 = reader.ReadString();
                        break;
                    case 2:
                        Gender		 = (Gender)reader.ReadInt32();
                        break;
                    case 4:
                        Dollar		 = reader.ReadInt32();
                        break;
                    case 5:
                        Coin			 = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               if(NickName	 != null)
               {
                   ProtoWriter.WriteFieldHeader(1, WireType.String, writer);
                   ProtoWriter.WriteString(NickName	, writer);
               }
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Gender		, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Dollar		, writer);
           }
           if(memberSets.ContainsKey(5))
           {
               ProtoWriter.WriteFieldHeader(5, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Coin			, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if(NickName	 != null)
            {
                sb.Append("NickName	:" + NickName	.ToString());
                sb.Append(",");
            }
            sb.Append("Gender		:" + Gender		.ToString());
            sb.Append(",");
            sb.Append("Dollar		:" + Dollar		.ToString());
            sb.Append(",");
            sb.Append("Coin			:" + Coin			.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 系统时间 登陆成功后发送  每分钟更新时候发送
    /// </summary>
    public partial class SystemTime : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Time"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SystemTime, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _time;
        public long Time
        {
           get { return _time;}
           set
           { 
               _time = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Time = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Time, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Time:" + Time.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum AwardType
    {
        Coin = 1,
        Dollar = 2,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class AwardData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
            ["Count"] = 3,
            ["CatName"] = 4,
            ["CatCode"] = 5,
            ["FurniCode"] = 6,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<AwardData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id			;
        public long Id			
        {
           get { return _id			;}
           set
           { 
               _id			 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private AwardType _type		;
        public AwardType Type		
        {
           get { return _type		;}
           set
           { 
               _type		 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _count		;
        public int Count		
        {
           get { return _count		;}
           set
           { 
               _count		 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private string _catname		;
        public string CatName		
        {
           get { return _catname		;}
           set
           { 
               _catname		 = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private string _catcode		;
        public string CatCode		
        {
           get { return _catcode		;}
           set
           { 
               _catcode		 = value;
               if(!memberSets.ContainsKey(5))
               { 
                    memberSets[5] = 1;
               } 
               else
               { 
                   memberSets[5]++;
               }
           }
        }

        private string _furnicode	;
        public string FurniCode	
        {
           get { return _furnicode	;}
           set
           { 
               _furnicode	 = value;
               if(!memberSets.ContainsKey(6))
               { 
                    memberSets[6] = 1;
               } 
               else
               { 
                   memberSets[6]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id			 = reader.ReadInt64();
                        break;
                    case 2:
                        Type		 = (AwardType)reader.ReadInt32();
                        break;
                    case 3:
                        Count		 = reader.ReadInt32();
                        break;
                    case 4:
                        CatName		 = reader.ReadString();
                        break;
                    case 5:
                        CatCode		 = reader.ReadString();
                        break;
                    case 6:
                        FurniCode	 = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id			, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Type		, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count		, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               if(CatName		 != null)
               {
                   ProtoWriter.WriteFieldHeader(4, WireType.String, writer);
                   ProtoWriter.WriteString(CatName		, writer);
               }
           }
           if(memberSets.ContainsKey(5))
           {
               if(CatCode		 != null)
               {
                   ProtoWriter.WriteFieldHeader(5, WireType.String, writer);
                   ProtoWriter.WriteString(CatCode		, writer);
               }
           }
           if(memberSets.ContainsKey(6))
           {
               if(FurniCode	 != null)
               {
                   ProtoWriter.WriteFieldHeader(6, WireType.String, writer);
                   ProtoWriter.WriteString(FurniCode	, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id			:" + Id			.ToString());
            sb.Append(",");
            sb.Append("Type		:" + Type		.ToString());
            sb.Append(",");
            sb.Append("Count		:" + Count		.ToString());
            sb.Append(",");
            if(CatName		 != null)
            {
                sb.Append("CatName		:" + CatName		.ToString());
                sb.Append(",");
            }
            if(CatCode		 != null)
            {
                sb.Append("CatCode		:" + CatCode		.ToString());
                sb.Append(",");
            }
            if(FurniCode	 != null)
            {
                sb.Append("FurniCode	:" + FurniCode	.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 登陆时候发送所有相关信息 如无特殊说明不会发其他信息
    /// </summary>
    public partial class EnterGameData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Character"] = 1,
            ["Guide"] = 2,
            ["Items"] = 3,
            ["Furnis"] = 4,
            ["Foods"] = 5,
            ["Awards"] = 6,
            ["Pics"] = 7,
            ["Rooms"] = 8,
            ["NRMCount"] = 9,
            ["SMMsgs"] = 10,
            ["Achieves"] = 11,
            ["Sign"] = 12,
            ["Shield"] = 13,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<EnterGameData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private CharacterData _character	;
        public CharacterData Character	
        {
           get { return _character	;}
           set
           { 
               _character	 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private GuideData _guide		;
        public GuideData Guide		
        {
           get { return _guide		;}
           set
           { 
               _guide		 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private List<ItemData> _items		;
        public List<ItemData> Items		
        {
           get
           {
               if(_items		==null)
               {
                   _items		 = new List<ItemData>();
               }
               return _items		;
           }
           private set { _items		=value;}
        }

        private List<FurniData> _furnis		;
        public List<FurniData> Furnis		
        {
           get
           {
               if(_furnis		==null)
               {
                   _furnis		 = new List<FurniData>();
               }
               return _furnis		;
           }
           private set { _furnis		=value;}
        }

        private List<FoodData> _foods		;
        public List<FoodData> Foods		
        {
           get
           {
               if(_foods		==null)
               {
                   _foods		 = new List<FoodData>();
               }
               return _foods		;
           }
           private set { _foods		=value;}
        }

        private List<AwardData> _awards		;
        public List<AwardData> Awards		
        {
           get
           {
               if(_awards		==null)
               {
                   _awards		 = new List<AwardData>();
               }
               return _awards		;
           }
           private set { _awards		=value;}
        }

        private List<PicData> _pics		;
        public List<PicData> Pics		
        {
           get
           {
               if(_pics		==null)
               {
                   _pics		 = new List<PicData>();
               }
               return _pics		;
           }
           private set { _pics		=value;}
        }

        private List<RoomData> _rooms		;
        public List<RoomData> Rooms		
        {
           get
           {
               if(_rooms		==null)
               {
                   _rooms		 = new List<RoomData>();
               }
               return _rooms		;
           }
           private set { _rooms		=value;}
        }

        private int _nrmcount	;
        /// <summary>
        /// 未读邮件数
        /// </summary>
        public int NRMCount	
        {
           get { return _nrmcount	;}
           set
           { 
               _nrmcount	 = value;
               if(!memberSets.ContainsKey(9))
               { 
                    memberSets[9] = 1;
               } 
               else
               { 
                   memberSets[9]++;
               }
           }
        }

        private List<SMMessage> _smmsgs		;
        /// <summary>
        /// 弹窗公告
        /// </summary>
        public List<SMMessage> SMMsgs		
        {
           get
           {
               if(_smmsgs		==null)
               {
                   _smmsgs		 = new List<SMMessage>();
               }
               return _smmsgs		;
           }
           private set { _smmsgs		=value;}
        }

        private List<AchievementData> _achieves;
        /// <summary>
        /// 成就
        /// </summary>
        public List<AchievementData> Achieves
        {
           get
           {
               if(_achieves==null)
               {
                   _achieves = new List<AchievementData>();
               }
               return _achieves;
           }
           private set { _achieves=value;}
        }

        private SignData _sign;
        /// <summary>
        /// 签到
        /// </summary>
        public SignData Sign
        {
           get { return _sign;}
           set
           { 
               _sign = value;
               if(!memberSets.ContainsKey(12))
               { 
                    memberSets[12] = 1;
               } 
               else
               { 
                   memberSets[12]++;
               }
           }
        }

        private bool _shield;
        /// <summary>
        /// 隐藏功能
        /// </summary>
        public bool Shield
        {
           get { return _shield;}
           set
           { 
               _shield = value;
               if(!memberSets.ContainsKey(13))
               { 
                    memberSets[13] = 1;
               } 
               else
               { 
                   memberSets[13]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempCharacter	 = new CharacterData();
                        var _tokenCharacter	 = ProtoReader.StartSubItem(reader);
                         _tempCharacter	.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenCharacter	, reader);
                        Character	 = _tempCharacter	;
                        break;
                    case 2:
                        var _tempGuide		 = new GuideData();
                        var _tokenGuide		 = ProtoReader.StartSubItem(reader);
                         _tempGuide		.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenGuide		, reader);
                        Guide		 = _tempGuide		;
                        break;
                    case 3:
                        var _tempItems		 = new ItemData();
                        var _tokenItems		 = ProtoReader.StartSubItem(reader);
                        _tempItems		.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenItems		, reader);
                        Items		.Add(_tempItems		);
                        break;
                    case 4:
                        var _tempFurnis		 = new FurniData();
                        var _tokenFurnis		 = ProtoReader.StartSubItem(reader);
                        _tempFurnis		.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenFurnis		, reader);
                        Furnis		.Add(_tempFurnis		);
                        break;
                    case 5:
                        var _tempFoods		 = new FoodData();
                        var _tokenFoods		 = ProtoReader.StartSubItem(reader);
                        _tempFoods		.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenFoods		, reader);
                        Foods		.Add(_tempFoods		);
                        break;
                    case 6:
                        var _tempAwards		 = new AwardData();
                        var _tokenAwards		 = ProtoReader.StartSubItem(reader);
                        _tempAwards		.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenAwards		, reader);
                        Awards		.Add(_tempAwards		);
                        break;
                    case 7:
                        var _tempPics		 = new PicData();
                        var _tokenPics		 = ProtoReader.StartSubItem(reader);
                        _tempPics		.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenPics		, reader);
                        Pics		.Add(_tempPics		);
                        break;
                    case 8:
                        var _tempRooms		 = new RoomData();
                        var _tokenRooms		 = ProtoReader.StartSubItem(reader);
                        _tempRooms		.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenRooms		, reader);
                        Rooms		.Add(_tempRooms		);
                        break;
                    case 9:
                        NRMCount	 = reader.ReadInt32();
                        break;
                    case 10:
                        var _tempSMMsgs		 = new SMMessage();
                        var _tokenSMMsgs		 = ProtoReader.StartSubItem(reader);
                        _tempSMMsgs		.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenSMMsgs		, reader);
                        SMMsgs		.Add(_tempSMMsgs		);
                        break;
                    case 11:
                        var _tempAchieves = new AchievementData();
                        var _tokenAchieves = ProtoReader.StartSubItem(reader);
                        _tempAchieves.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenAchieves, reader);
                        Achieves.Add(_tempAchieves);
                        break;
                    case 12:
                        var _tempSign = new SignData();
                        var _tokenSign = ProtoReader.StartSubItem(reader);
                         _tempSign.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenSign, reader);
                        Sign = _tempSign;
                        break;
                    case 13:
                        Shield = reader.ReadBoolean();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               if(Character	 != null)
               {
                   ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenCharacter	 = ProtoWriter.StartSubItem(Character	, writer);
                  Character	.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenCharacter	, writer);
               }
           }
           if(memberSets.ContainsKey(2))
           {
               if(Guide		 != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.StartGroup, writer);
                  var _tokenGuide		 = ProtoWriter.StartSubItem(Guide		, writer);
                  Guide		.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenGuide		, writer);
               }
           }
           if(_items		 != null && _items		.Count > 0)
           {
              for(int i = 0; i < Items		.Count; i++)
              {
                  var temp = Items		[i];
                  ProtoWriter.WriteFieldHeader(3, WireType.StartGroup, writer);
                  var _tokenItems		 = ProtoWriter.StartSubItem(Items		, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenItems		, writer);
              }
           }
           if(_furnis		 != null && _furnis		.Count > 0)
           {
              for(int i = 0; i < Furnis		.Count; i++)
              {
                  var temp = Furnis		[i];
                  ProtoWriter.WriteFieldHeader(4, WireType.StartGroup, writer);
                  var _tokenFurnis		 = ProtoWriter.StartSubItem(Furnis		, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenFurnis		, writer);
              }
           }
           if(_foods		 != null && _foods		.Count > 0)
           {
              for(int i = 0; i < Foods		.Count; i++)
              {
                  var temp = Foods		[i];
                  ProtoWriter.WriteFieldHeader(5, WireType.StartGroup, writer);
                  var _tokenFoods		 = ProtoWriter.StartSubItem(Foods		, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenFoods		, writer);
              }
           }
           if(_awards		 != null && _awards		.Count > 0)
           {
              for(int i = 0; i < Awards		.Count; i++)
              {
                  var temp = Awards		[i];
                  ProtoWriter.WriteFieldHeader(6, WireType.StartGroup, writer);
                  var _tokenAwards		 = ProtoWriter.StartSubItem(Awards		, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenAwards		, writer);
              }
           }
           if(_pics		 != null && _pics		.Count > 0)
           {
              for(int i = 0; i < Pics		.Count; i++)
              {
                  var temp = Pics		[i];
                  ProtoWriter.WriteFieldHeader(7, WireType.StartGroup, writer);
                  var _tokenPics		 = ProtoWriter.StartSubItem(Pics		, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenPics		, writer);
              }
           }
           if(_rooms		 != null && _rooms		.Count > 0)
           {
              for(int i = 0; i < Rooms		.Count; i++)
              {
                  var temp = Rooms		[i];
                  ProtoWriter.WriteFieldHeader(8, WireType.StartGroup, writer);
                  var _tokenRooms		 = ProtoWriter.StartSubItem(Rooms		, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenRooms		, writer);
              }
           }
           if(memberSets.ContainsKey(9))
           {
               ProtoWriter.WriteFieldHeader(9, WireType.Variant, writer);
               ProtoWriter.WriteInt32(NRMCount	, writer);
           }
           if(_smmsgs		 != null && _smmsgs		.Count > 0)
           {
              for(int i = 0; i < SMMsgs		.Count; i++)
              {
                  var temp = SMMsgs		[i];
                  ProtoWriter.WriteFieldHeader(10, WireType.StartGroup, writer);
                  var _tokenSMMsgs		 = ProtoWriter.StartSubItem(SMMsgs		, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenSMMsgs		, writer);
              }
           }
           if(_achieves != null && _achieves.Count > 0)
           {
              for(int i = 0; i < Achieves.Count; i++)
              {
                  var temp = Achieves[i];
                  ProtoWriter.WriteFieldHeader(11, WireType.StartGroup, writer);
                  var _tokenAchieves = ProtoWriter.StartSubItem(Achieves, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenAchieves, writer);
              }
           }
           if(memberSets.ContainsKey(12))
           {
               if(Sign != null)
               {
                   ProtoWriter.WriteFieldHeader(12, WireType.StartGroup, writer);
                  var _tokenSign = ProtoWriter.StartSubItem(Sign, writer);
                  Sign.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenSign, writer);
               }
           }
           if(memberSets.ContainsKey(13))
           {
               ProtoWriter.WriteFieldHeader(13, WireType.Variant, writer);
               ProtoWriter.WriteBoolean(Shield, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if(Character	 != null)
            {
                sb.Append("Character	:" + Character	.ToString());
                sb.Append(",");
            }
            if(Guide		 != null)
            {
                sb.Append("Guide		:" + Guide		.ToString());
                sb.Append(",");
            }
           if(_items		 != null && _items		.Count > 0)
           {
              sb.Append("Items		:[");
              for(int i = 0; i < Items		.Count; i++)
              {
                  sb.Append(Items		[i].ToString());
              }
              sb.Append("],");
           }
           if(_furnis		 != null && _furnis		.Count > 0)
           {
              sb.Append("Furnis		:[");
              for(int i = 0; i < Furnis		.Count; i++)
              {
                  sb.Append(Furnis		[i].ToString());
              }
              sb.Append("],");
           }
           if(_foods		 != null && _foods		.Count > 0)
           {
              sb.Append("Foods		:[");
              for(int i = 0; i < Foods		.Count; i++)
              {
                  sb.Append(Foods		[i].ToString());
              }
              sb.Append("],");
           }
           if(_awards		 != null && _awards		.Count > 0)
           {
              sb.Append("Awards		:[");
              for(int i = 0; i < Awards		.Count; i++)
              {
                  sb.Append(Awards		[i].ToString());
              }
              sb.Append("],");
           }
           if(_pics		 != null && _pics		.Count > 0)
           {
              sb.Append("Pics		:[");
              for(int i = 0; i < Pics		.Count; i++)
              {
                  sb.Append(Pics		[i].ToString());
              }
              sb.Append("],");
           }
           if(_rooms		 != null && _rooms		.Count > 0)
           {
              sb.Append("Rooms		:[");
              for(int i = 0; i < Rooms		.Count; i++)
              {
                  sb.Append(Rooms		[i].ToString());
              }
              sb.Append("],");
           }
            sb.Append("NRMCount	:" + NRMCount	.ToString());
            sb.Append(",");
           if(_smmsgs		 != null && _smmsgs		.Count > 0)
           {
              sb.Append("SMMsgs		:[");
              for(int i = 0; i < SMMsgs		.Count; i++)
              {
                  sb.Append(SMMsgs		[i].ToString());
              }
              sb.Append("],");
           }
           if(_achieves != null && _achieves.Count > 0)
           {
              sb.Append("Achieves:[");
              for(int i = 0; i < Achieves.Count; i++)
              {
                  sb.Append(Achieves[i].ToString());
              }
              sb.Append("],");
           }
            if(Sign != null)
            {
                sb.Append("Sign:" + Sign.ToString());
                sb.Append(",");
            }
            sb.Append("Shield:" + Shield.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 角色的一些数值
    /// </summary>
    public partial class CharacterData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Name"] = 2,
            ["Gender"] = 3,
            ["Dollar"] = 4,
            ["Coin"] = 5,
            ["AdminType"] = 6,
            ["Achieve"] = 7,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<CharacterData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id			;
        public long Id			
        {
           get { return _id			;}
           set
           { 
               _id			 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private string _name		;
        public string Name		
        {
           get { return _name		;}
           set
           { 
               _name		 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _gender		;
        public int Gender		
        {
           get { return _gender		;}
           set
           { 
               _gender		 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private int _dollar		 = -1;
        public int Dollar		
        {
           get { return _dollar		;}
           set
           { 
               _dollar		 = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private long _coin			 = -1;
        public long Coin			
        {
           get { return _coin			;}
           set
           { 
               _coin			 = value;
               if(!memberSets.ContainsKey(5))
               { 
                    memberSets[5] = 1;
               } 
               else
               { 
                   memberSets[5]++;
               }
           }
        }

        private int _admintype	 = -1;
        public int AdminType	
        {
           get { return _admintype	;}
           set
           { 
               _admintype	 = value;
               if(!memberSets.ContainsKey(6))
               { 
                    memberSets[6] = 1;
               } 
               else
               { 
                   memberSets[6]++;
               }
           }
        }

        private long _achieve;
        public long Achieve
        {
           get { return _achieve;}
           set
           { 
               _achieve = value;
               if(!memberSets.ContainsKey(7))
               { 
                    memberSets[7] = 1;
               } 
               else
               { 
                   memberSets[7]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id			 = reader.ReadInt64();
                        break;
                    case 2:
                        Name		 = reader.ReadString();
                        break;
                    case 3:
                        Gender		 = reader.ReadInt32();
                        break;
                    case 4:
                        Dollar		 = reader.ReadInt32();
                        break;
                    case 5:
                        Coin			 = reader.ReadInt64();
                        break;
                    case 6:
                        AdminType	 = reader.ReadInt32();
                        break;
                    case 7:
                        Achieve = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id			, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               if(Name		 != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.String, writer);
                   ProtoWriter.WriteString(Name		, writer);
               }
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Gender		, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Dollar		, writer);
           }
           if(memberSets.ContainsKey(5))
           {
               ProtoWriter.WriteFieldHeader(5, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Coin			, writer);
           }
           if(memberSets.ContainsKey(6))
           {
               ProtoWriter.WriteFieldHeader(6, WireType.Variant, writer);
               ProtoWriter.WriteInt32(AdminType	, writer);
           }
           if(memberSets.ContainsKey(7))
           {
               ProtoWriter.WriteFieldHeader(7, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Achieve, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id			:" + Id			.ToString());
            sb.Append(",");
            if(Name		 != null)
            {
                sb.Append("Name		:" + Name		.ToString());
                sb.Append(",");
            }
            sb.Append("Gender		:" + Gender		.ToString());
            sb.Append(",");
            sb.Append("Dollar		:" + Dollar		.ToString());
            sb.Append(",");
            sb.Append("Coin			:" + Coin			.ToString());
            sb.Append(",");
            sb.Append("AdminType	:" + AdminType	.ToString());
            sb.Append(",");
            sb.Append("Achieve:" + Achieve.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 道具信息
    /// </summary>
    public partial class ItemData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
            ["Count"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<ItemData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _type;
        public long Type
        {
           get { return _type;}
           set
           { 
               _type = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _count	;
        public int Count	
        {
           get { return _count	;}
           set
           { 
               _count	 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Type = reader.ReadInt64();
                        break;
                    case 3:
                        Count	 = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("Type:" + Type.ToString());
            sb.Append(",");
            sb.Append("Count	:" + Count	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 家具信息
    /// </summary>
    public partial class SpreadData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Path"] = 1,
            ["CatID"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SpreadData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private string _path;
        public string Path
        {
           get { return _path;}
           set
           { 
               _path = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _catid;
        public long CatID
        {
           get { return _catid;}
           set
           { 
               _catid = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Path = reader.ReadString();
                        break;
                    case 2:
                        CatID = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               if(Path != null)
               {
                   ProtoWriter.WriteFieldHeader(1, WireType.String, writer);
                   ProtoWriter.WriteString(Path, writer);
               }
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(CatID, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if(Path != null)
            {
                sb.Append("Path:" + Path.ToString());
                sb.Append(",");
            }
            sb.Append("CatID:" + CatID.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class FurniData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
            ["Count"] = 3,
            ["RoomID"] = 4,
            ["ScenePointIndex"] = 5,
            ["SubPointIndex"] = 6,
            ["RoomSectionType"] = 7,
            ["Speaks"] = 8,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<FurniData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _type;
        public long Type
        {
           get { return _type;}
           set
           { 
               _type = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _count;
        public int Count
        {
           get { return _count;}
           set
           { 
               _count = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private long _roomid;
        public long RoomID
        {
           get { return _roomid;}
           set
           { 
               _roomid = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private int _scenepointindex;
        public int ScenePointIndex
        {
           get { return _scenepointindex;}
           set
           { 
               _scenepointindex = value;
               if(!memberSets.ContainsKey(5))
               { 
                    memberSets[5] = 1;
               } 
               else
               { 
                   memberSets[5]++;
               }
           }
        }

        private int _subpointindex;
        public int SubPointIndex
        {
           get { return _subpointindex;}
           set
           { 
               _subpointindex = value;
               if(!memberSets.ContainsKey(6))
               { 
                    memberSets[6] = 1;
               } 
               else
               { 
                   memberSets[6]++;
               }
           }
        }

        private RoomSectionType _roomsectiontype	;
        public RoomSectionType RoomSectionType	
        {
           get { return _roomsectiontype	;}
           set
           { 
               _roomsectiontype	 = value;
               if(!memberSets.ContainsKey(7))
               { 
                    memberSets[7] = 1;
               } 
               else
               { 
                   memberSets[7]++;
               }
           }
        }

        private List<SpreadData> _speaks;
        public List<SpreadData> Speaks
        {
           get
           {
               if(_speaks==null)
               {
                   _speaks = new List<SpreadData>();
               }
               return _speaks;
           }
           private set { _speaks=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Type = reader.ReadInt64();
                        break;
                    case 3:
                        Count = reader.ReadInt32();
                        break;
                    case 4:
                        RoomID = reader.ReadInt64();
                        break;
                    case 5:
                        ScenePointIndex = reader.ReadInt32();
                        break;
                    case 6:
                        SubPointIndex = reader.ReadInt32();
                        break;
                    case 7:
                        RoomSectionType	 = (RoomSectionType)reader.ReadInt32();
                        break;
                    case 8:
                        var _tempSpeaks = new SpreadData();
                        var _tokenSpeaks = ProtoReader.StartSubItem(reader);
                        _tempSpeaks.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenSpeaks, reader);
                        Speaks.Add(_tempSpeaks);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt64(RoomID, writer);
           }
           if(memberSets.ContainsKey(5))
           {
               ProtoWriter.WriteFieldHeader(5, WireType.Variant, writer);
               ProtoWriter.WriteInt32(ScenePointIndex, writer);
           }
           if(memberSets.ContainsKey(6))
           {
               ProtoWriter.WriteFieldHeader(6, WireType.Variant, writer);
               ProtoWriter.WriteInt32(SubPointIndex, writer);
           }
           if(memberSets.ContainsKey(7))
           {
               ProtoWriter.WriteFieldHeader(7, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)RoomSectionType	, writer);
           }
           if(_speaks != null && _speaks.Count > 0)
           {
              for(int i = 0; i < Speaks.Count; i++)
              {
                  var temp = Speaks[i];
                  ProtoWriter.WriteFieldHeader(8, WireType.StartGroup, writer);
                  var _tokenSpeaks = ProtoWriter.StartSubItem(Speaks, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenSpeaks, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("Type:" + Type.ToString());
            sb.Append(",");
            sb.Append("Count:" + Count.ToString());
            sb.Append(",");
            sb.Append("RoomID:" + RoomID.ToString());
            sb.Append(",");
            sb.Append("ScenePointIndex:" + ScenePointIndex.ToString());
            sb.Append(",");
            sb.Append("SubPointIndex:" + SubPointIndex.ToString());
            sb.Append(",");
            sb.Append("RoomSectionType	:" + RoomSectionType	.ToString());
            sb.Append(",");
           if(_speaks != null && _speaks.Count > 0)
           {
              sb.Append("Speaks:[");
              for(int i = 0; i < Speaks.Count; i++)
              {
                  sb.Append(Speaks[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum FoodLevel
    {
        _1 = 1,
        _2 = 2,
        _3 = 3,
        _4 = 4,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class FoodData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
            ["Count"] = 3,
            ["RoomID"] = 4,
            ["ScenePointIndex"] = 5,
            ["SubPointIndex"] = 6,
            ["RoomSectionType"] = 7,
            ["Duration"] = 8,
            ["LastPlaceTime"] = 9,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<FoodData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _type;
        public long Type
        {
           get { return _type;}
           set
           { 
               _type = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private long _count;
        public long Count
        {
           get { return _count;}
           set
           { 
               _count = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private long _roomid;
        public long RoomID
        {
           get { return _roomid;}
           set
           { 
               _roomid = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private int _scenepointindex;
        public int ScenePointIndex
        {
           get { return _scenepointindex;}
           set
           { 
               _scenepointindex = value;
               if(!memberSets.ContainsKey(5))
               { 
                    memberSets[5] = 1;
               } 
               else
               { 
                   memberSets[5]++;
               }
           }
        }

        private int _subpointindex;
        public int SubPointIndex
        {
           get { return _subpointindex;}
           set
           { 
               _subpointindex = value;
               if(!memberSets.ContainsKey(6))
               { 
                    memberSets[6] = 1;
               } 
               else
               { 
                   memberSets[6]++;
               }
           }
        }

        private RoomSectionType _roomsectiontype	;
        public RoomSectionType RoomSectionType	
        {
           get { return _roomsectiontype	;}
           set
           { 
               _roomsectiontype	 = value;
               if(!memberSets.ContainsKey(7))
               { 
                    memberSets[7] = 1;
               } 
               else
               { 
                   memberSets[7]++;
               }
           }
        }

        private long _duration;
        public long Duration
        {
           get { return _duration;}
           set
           { 
               _duration = value;
               if(!memberSets.ContainsKey(8))
               { 
                    memberSets[8] = 1;
               } 
               else
               { 
                   memberSets[8]++;
               }
           }
        }

        private long _lastplacetime;
        public long LastPlaceTime
        {
           get { return _lastplacetime;}
           set
           { 
               _lastplacetime = value;
               if(!memberSets.ContainsKey(9))
               { 
                    memberSets[9] = 1;
               } 
               else
               { 
                   memberSets[9]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Type = reader.ReadInt64();
                        break;
                    case 3:
                        Count = reader.ReadInt64();
                        break;
                    case 4:
                        RoomID = reader.ReadInt64();
                        break;
                    case 5:
                        ScenePointIndex = reader.ReadInt32();
                        break;
                    case 6:
                        SubPointIndex = reader.ReadInt32();
                        break;
                    case 7:
                        RoomSectionType	 = (RoomSectionType)reader.ReadInt32();
                        break;
                    case 8:
                        Duration = reader.ReadInt64();
                        break;
                    case 9:
                        LastPlaceTime = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Count, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt64(RoomID, writer);
           }
           if(memberSets.ContainsKey(5))
           {
               ProtoWriter.WriteFieldHeader(5, WireType.Variant, writer);
               ProtoWriter.WriteInt32(ScenePointIndex, writer);
           }
           if(memberSets.ContainsKey(6))
           {
               ProtoWriter.WriteFieldHeader(6, WireType.Variant, writer);
               ProtoWriter.WriteInt32(SubPointIndex, writer);
           }
           if(memberSets.ContainsKey(7))
           {
               ProtoWriter.WriteFieldHeader(7, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)RoomSectionType	, writer);
           }
           if(memberSets.ContainsKey(8))
           {
               ProtoWriter.WriteFieldHeader(8, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Duration, writer);
           }
           if(memberSets.ContainsKey(9))
           {
               ProtoWriter.WriteFieldHeader(9, WireType.Variant, writer);
               ProtoWriter.WriteInt64(LastPlaceTime, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("Type:" + Type.ToString());
            sb.Append(",");
            sb.Append("Count:" + Count.ToString());
            sb.Append(",");
            sb.Append("RoomID:" + RoomID.ToString());
            sb.Append(",");
            sb.Append("ScenePointIndex:" + ScenePointIndex.ToString());
            sb.Append(",");
            sb.Append("SubPointIndex:" + SubPointIndex.ToString());
            sb.Append(",");
            sb.Append("RoomSectionType	:" + RoomSectionType	.ToString());
            sb.Append(",");
            sb.Append("Duration:" + Duration.ToString());
            sb.Append(",");
            sb.Append("LastPlaceTime:" + LastPlaceTime.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum PicShowType
    {
        _1 = 1,
        _2 = 2,
        _3 = 3,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class PicData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
            ["ShowTimes"] = 3,
            ["IsFound"] = 4,
            ["LastShowTime"] = 5,
            ["TreasureID"] = 6,
            ["IsSentTreasure"] = 7,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<PicData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id				;
        public long Id				
        {
           get { return _id				;}
           set
           { 
               _id				 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _type				;
        public long Type				
        {
           get { return _type				;}
           set
           { 
               _type				 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private long _showtimes		;
        public long ShowTimes		
        {
           get { return _showtimes		;}
           set
           { 
               _showtimes		 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private long _isfound			;
        public long IsFound			
        {
           get { return _isfound			;}
           set
           { 
               _isfound			 = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private string _lastshowtime	;
        public string LastShowTime	
        {
           get { return _lastshowtime	;}
           set
           { 
               _lastshowtime	 = value;
               if(!memberSets.ContainsKey(5))
               { 
                    memberSets[5] = 1;
               } 
               else
               { 
                   memberSets[5]++;
               }
           }
        }

        private long _treasureid		;
        public long TreasureID		
        {
           get { return _treasureid		;}
           set
           { 
               _treasureid		 = value;
               if(!memberSets.ContainsKey(6))
               { 
                    memberSets[6] = 1;
               } 
               else
               { 
                   memberSets[6]++;
               }
           }
        }

        private bool _issenttreasure	;
        public bool IsSentTreasure	
        {
           get { return _issenttreasure	;}
           set
           { 
               _issenttreasure	 = value;
               if(!memberSets.ContainsKey(7))
               { 
                    memberSets[7] = 1;
               } 
               else
               { 
                   memberSets[7]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id				 = reader.ReadInt64();
                        break;
                    case 2:
                        Type				 = reader.ReadInt64();
                        break;
                    case 3:
                        ShowTimes		 = reader.ReadInt64();
                        break;
                    case 4:
                        IsFound			 = reader.ReadInt64();
                        break;
                    case 5:
                        LastShowTime	 = reader.ReadString();
                        break;
                    case 6:
                        TreasureID		 = reader.ReadInt64();
                        break;
                    case 7:
                        IsSentTreasure	 = reader.ReadBoolean();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id				, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type				, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt64(ShowTimes		, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt64(IsFound			, writer);
           }
           if(memberSets.ContainsKey(5))
           {
               if(LastShowTime	 != null)
               {
                   ProtoWriter.WriteFieldHeader(5, WireType.String, writer);
                   ProtoWriter.WriteString(LastShowTime	, writer);
               }
           }
           if(memberSets.ContainsKey(6))
           {
               ProtoWriter.WriteFieldHeader(6, WireType.Variant, writer);
               ProtoWriter.WriteInt64(TreasureID		, writer);
           }
           if(memberSets.ContainsKey(7))
           {
               ProtoWriter.WriteFieldHeader(7, WireType.Variant, writer);
               ProtoWriter.WriteBoolean(IsSentTreasure	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id				:" + Id				.ToString());
            sb.Append(",");
            sb.Append("Type				:" + Type				.ToString());
            sb.Append(",");
            sb.Append("ShowTimes		:" + ShowTimes		.ToString());
            sb.Append(",");
            sb.Append("IsFound			:" + IsFound			.ToString());
            sb.Append(",");
            if(LastShowTime	 != null)
            {
                sb.Append("LastShowTime	:" + LastShowTime	.ToString());
                sb.Append(",");
            }
            sb.Append("TreasureID		:" + TreasureID		.ToString());
            sb.Append(",");
            sb.Append("IsSentTreasure	:" + IsSentTreasure	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class PicDatas : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Datas"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<PicDatas, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private List<PicData> _datas	;
        public List<PicData> Datas	
        {
           get
           {
               if(_datas	==null)
               {
                   _datas	 = new List<PicData>();
               }
               return _datas	;
           }
           private set { _datas	=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempDatas	 = new PicData();
                        var _tokenDatas	 = ProtoReader.StartSubItem(reader);
                        _tempDatas	.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenDatas	, reader);
                        Datas	.Add(_tempDatas	);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(_datas	 != null && _datas	.Count > 0)
           {
              for(int i = 0; i < Datas	.Count; i++)
              {
                  var temp = Datas	[i];
                  ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenDatas	 = ProtoWriter.StartSubItem(Datas	, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenDatas	, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
           if(_datas	 != null && _datas	.Count > 0)
           {
              sb.Append("Datas	:[");
              for(int i = 0; i < Datas	.Count; i++)
              {
                  sb.Append(Datas	[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class TreasureData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<TreasureData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id	;
        public long Id	
        {
           get { return _id	;}
           set
           { 
               _id	 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private List<long> _type	;
        public List<long> Type	
        {
           get
           {
               if(_type	==null)
               {
                   _type	 = new List<long>();
               }
               return _type	;
           }
           private set { _type	=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id	 = reader.ReadInt64();
                        break;
                    case 2:
                        var _tempType	 = reader.ReadInt64();
                        Type	.Add(_tempType	);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id	, writer);
           }
           if(_type	 != null && _type	.Count > 0)
           {
              for(int i = 0; i < Type	.Count; i++)
              {
                  var temp = Type	[i];
                  ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
                  ProtoWriter.WriteInt64(temp, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id	:" + Id	.ToString());
            sb.Append(",");
           if(_type	 != null && _type	.Count > 0)
           {
              sb.Append("Type	:[");
              for(int i = 0; i < Type	.Count; i++)
              {
                  sb.Append(Type	[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class TreasureDatas : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Datas"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<TreasureDatas, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private List<TreasureData> _datas;
        public List<TreasureData> Datas
        {
           get
           {
               if(_datas==null)
               {
                   _datas = new List<TreasureData>();
               }
               return _datas;
           }
           private set { _datas=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempDatas = new TreasureData();
                        var _tokenDatas = ProtoReader.StartSubItem(reader);
                        _tempDatas.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenDatas, reader);
                        Datas.Add(_tempDatas);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(_datas != null && _datas.Count > 0)
           {
              for(int i = 0; i < Datas.Count; i++)
              {
                  var temp = Datas[i];
                  ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenDatas = ProtoWriter.StartSubItem(Datas, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenDatas, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
           if(_datas != null && _datas.Count > 0)
           {
              sb.Append("Datas:[");
              for(int i = 0; i < Datas.Count; i++)
              {
                  sb.Append(Datas[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// ***猫***/
    /// </summary>
    public partial class CatData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
            ["Name"] = 3,
            ["Animation"] = 4,
            ["Quality"] = 5,
            ["RoomID"] = 6,
            ["RootID"] = 7,
            ["Gesture"] = 8,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<CatData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id			;
        public long Id			
        {
           get { return _id			;}
           set
           { 
               _id			 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _type			;
        public long Type			
        {
           get { return _type			;}
           set
           { 
               _type			 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private string _name		;
        public string Name		
        {
           get { return _name		;}
           set
           { 
               _name		 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private string _animation	;
        public string Animation	
        {
           get { return _animation	;}
           set
           { 
               _animation	 = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private CatQuality _quality	;
        public CatQuality Quality	
        {
           get { return _quality	;}
           set
           { 
               _quality	 = value;
               if(!memberSets.ContainsKey(5))
               { 
                    memberSets[5] = 1;
               } 
               else
               { 
                   memberSets[5]++;
               }
           }
        }

        private long _roomid		;
        public long RoomID		
        {
           get { return _roomid		;}
           set
           { 
               _roomid		 = value;
               if(!memberSets.ContainsKey(6))
               { 
                    memberSets[6] = 1;
               } 
               else
               { 
                   memberSets[6]++;
               }
           }
        }

        private long _rootid		;
        public long RootID		
        {
           get { return _rootid		;}
           set
           { 
               _rootid		 = value;
               if(!memberSets.ContainsKey(7))
               { 
                    memberSets[7] = 1;
               } 
               else
               { 
                   memberSets[7]++;
               }
           }
        }

        private string _gesture		;
        public string Gesture		
        {
           get { return _gesture		;}
           set
           { 
               _gesture		 = value;
               if(!memberSets.ContainsKey(8))
               { 
                    memberSets[8] = 1;
               } 
               else
               { 
                   memberSets[8]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id			 = reader.ReadInt64();
                        break;
                    case 2:
                        Type			 = reader.ReadInt64();
                        break;
                    case 3:
                        Name		 = reader.ReadString();
                        break;
                    case 4:
                        Animation	 = reader.ReadString();
                        break;
                    case 5:
                        Quality	 = (CatQuality)reader.ReadInt32();
                        break;
                    case 6:
                        RoomID		 = reader.ReadInt64();
                        break;
                    case 7:
                        RootID		 = reader.ReadInt64();
                        break;
                    case 8:
                        Gesture		 = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id			, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type			, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               if(Name		 != null)
               {
                   ProtoWriter.WriteFieldHeader(3, WireType.String, writer);
                   ProtoWriter.WriteString(Name		, writer);
               }
           }
           if(memberSets.ContainsKey(4))
           {
               if(Animation	 != null)
               {
                   ProtoWriter.WriteFieldHeader(4, WireType.String, writer);
                   ProtoWriter.WriteString(Animation	, writer);
               }
           }
           if(memberSets.ContainsKey(5))
           {
               ProtoWriter.WriteFieldHeader(5, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Quality	, writer);
           }
           if(memberSets.ContainsKey(6))
           {
               ProtoWriter.WriteFieldHeader(6, WireType.Variant, writer);
               ProtoWriter.WriteInt64(RoomID		, writer);
           }
           if(memberSets.ContainsKey(7))
           {
               ProtoWriter.WriteFieldHeader(7, WireType.Variant, writer);
               ProtoWriter.WriteInt64(RootID		, writer);
           }
           if(memberSets.ContainsKey(8))
           {
               if(Gesture		 != null)
               {
                   ProtoWriter.WriteFieldHeader(8, WireType.String, writer);
                   ProtoWriter.WriteString(Gesture		, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id			:" + Id			.ToString());
            sb.Append(",");
            sb.Append("Type			:" + Type			.ToString());
            sb.Append(",");
            if(Name		 != null)
            {
                sb.Append("Name		:" + Name		.ToString());
                sb.Append(",");
            }
            if(Animation	 != null)
            {
                sb.Append("Animation	:" + Animation	.ToString());
                sb.Append(",");
            }
            sb.Append("Quality	:" + Quality	.ToString());
            sb.Append(",");
            sb.Append("RoomID		:" + RoomID		.ToString());
            sb.Append(",");
            sb.Append("RootID		:" + RootID		.ToString());
            sb.Append(",");
            if(Gesture		 != null)
            {
                sb.Append("Gesture		:" + Gesture		.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class GiftData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["PropId"] = 2,
            ["Description"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<GiftData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id			;
        public long Id			
        {
           get { return _id			;}
           set
           { 
               _id			 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _propid		;
        public long PropId		
        {
           get { return _propid		;}
           set
           { 
               _propid		 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private string _description	;
        public string Description	
        {
           get { return _description	;}
           set
           { 
               _description	 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id			 = reader.ReadInt64();
                        break;
                    case 2:
                        PropId		 = reader.ReadInt64();
                        break;
                    case 3:
                        Description	 = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id			, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(PropId		, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               if(Description	 != null)
               {
                   ProtoWriter.WriteFieldHeader(3, WireType.String, writer);
                   ProtoWriter.WriteString(Description	, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id			:" + Id			.ToString());
            sb.Append(",");
            sb.Append("PropId		:" + PropId		.ToString());
            sb.Append(",");
            if(Description	 != null)
            {
                sb.Append("Description	:" + Description	.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum CatQuality
    {
        普通 = 0,
        名品 = 1,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class RoomData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
            ["Name"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<RoomData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id		;
        public long Id		
        {
           get { return _id		;}
           set
           { 
               _id		 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _type		;
        public long Type		
        {
           get { return _type		;}
           set
           { 
               _type		 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private string _name	;
        public string Name	
        {
           get { return _name	;}
           set
           { 
               _name	 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id		 = reader.ReadInt64();
                        break;
                    case 2:
                        Type		 = reader.ReadInt64();
                        break;
                    case 3:
                        Name	 = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id		, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type		, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               if(Name	 != null)
               {
                   ProtoWriter.WriteFieldHeader(3, WireType.String, writer);
                   ProtoWriter.WriteString(Name	, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id		:" + Id		.ToString());
            sb.Append(",");
            sb.Append("Type		:" + Type		.ToString());
            sb.Append(",");
            if(Name	 != null)
            {
                sb.Append("Name	:" + Name	.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class EnterRoomData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
            ["Name"] = 3,
            ["Owner"] = 4,
            ["Furnis"] = 5,
            ["Foods"] = 6,
            ["Cats"] = 7,
            ["Treasures"] = 8,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<EnterRoomData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _type;
        public long Type
        {
           get { return _type;}
           set
           { 
               _type = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private string _name;
        public string Name
        {
           get { return _name;}
           set
           { 
               _name = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private long _owner;
        public long Owner
        {
           get { return _owner;}
           set
           { 
               _owner = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private List<FurniData> _furnis;
        public List<FurniData> Furnis
        {
           get
           {
               if(_furnis==null)
               {
                   _furnis = new List<FurniData>();
               }
               return _furnis;
           }
           private set { _furnis=value;}
        }

        private List<FoodData> _foods;
        public List<FoodData> Foods
        {
           get
           {
               if(_foods==null)
               {
                   _foods = new List<FoodData>();
               }
               return _foods;
           }
           private set { _foods=value;}
        }

        private List<CatData> _cats;
        public List<CatData> Cats
        {
           get
           {
               if(_cats==null)
               {
                   _cats = new List<CatData>();
               }
               return _cats;
           }
           private set { _cats=value;}
        }

        private List<TreasureData> _treasures	;
        public List<TreasureData> Treasures	
        {
           get
           {
               if(_treasures	==null)
               {
                   _treasures	 = new List<TreasureData>();
               }
               return _treasures	;
           }
           private set { _treasures	=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Type = reader.ReadInt64();
                        break;
                    case 3:
                        Name = reader.ReadString();
                        break;
                    case 4:
                        Owner = reader.ReadInt64();
                        break;
                    case 5:
                        var _tempFurnis = new FurniData();
                        var _tokenFurnis = ProtoReader.StartSubItem(reader);
                        _tempFurnis.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenFurnis, reader);
                        Furnis.Add(_tempFurnis);
                        break;
                    case 6:
                        var _tempFoods = new FoodData();
                        var _tokenFoods = ProtoReader.StartSubItem(reader);
                        _tempFoods.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenFoods, reader);
                        Foods.Add(_tempFoods);
                        break;
                    case 7:
                        var _tempCats = new CatData();
                        var _tokenCats = ProtoReader.StartSubItem(reader);
                        _tempCats.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenCats, reader);
                        Cats.Add(_tempCats);
                        break;
                    case 8:
                        var _tempTreasures	 = new TreasureData();
                        var _tokenTreasures	 = ProtoReader.StartSubItem(reader);
                        _tempTreasures	.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenTreasures	, reader);
                        Treasures	.Add(_tempTreasures	);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               if(Name != null)
               {
                   ProtoWriter.WriteFieldHeader(3, WireType.String, writer);
                   ProtoWriter.WriteString(Name, writer);
               }
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Owner, writer);
           }
           if(_furnis != null && _furnis.Count > 0)
           {
              for(int i = 0; i < Furnis.Count; i++)
              {
                  var temp = Furnis[i];
                  ProtoWriter.WriteFieldHeader(5, WireType.StartGroup, writer);
                  var _tokenFurnis = ProtoWriter.StartSubItem(Furnis, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenFurnis, writer);
              }
           }
           if(_foods != null && _foods.Count > 0)
           {
              for(int i = 0; i < Foods.Count; i++)
              {
                  var temp = Foods[i];
                  ProtoWriter.WriteFieldHeader(6, WireType.StartGroup, writer);
                  var _tokenFoods = ProtoWriter.StartSubItem(Foods, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenFoods, writer);
              }
           }
           if(_cats != null && _cats.Count > 0)
           {
              for(int i = 0; i < Cats.Count; i++)
              {
                  var temp = Cats[i];
                  ProtoWriter.WriteFieldHeader(7, WireType.StartGroup, writer);
                  var _tokenCats = ProtoWriter.StartSubItem(Cats, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenCats, writer);
              }
           }
           if(_treasures	 != null && _treasures	.Count > 0)
           {
              for(int i = 0; i < Treasures	.Count; i++)
              {
                  var temp = Treasures	[i];
                  ProtoWriter.WriteFieldHeader(8, WireType.StartGroup, writer);
                  var _tokenTreasures	 = ProtoWriter.StartSubItem(Treasures	, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenTreasures	, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("Type:" + Type.ToString());
            sb.Append(",");
            if(Name != null)
            {
                sb.Append("Name:" + Name.ToString());
                sb.Append(",");
            }
            sb.Append("Owner:" + Owner.ToString());
            sb.Append(",");
           if(_furnis != null && _furnis.Count > 0)
           {
              sb.Append("Furnis:[");
              for(int i = 0; i < Furnis.Count; i++)
              {
                  sb.Append(Furnis[i].ToString());
              }
              sb.Append("],");
           }
           if(_foods != null && _foods.Count > 0)
           {
              sb.Append("Foods:[");
              for(int i = 0; i < Foods.Count; i++)
              {
                  sb.Append(Foods[i].ToString());
              }
              sb.Append("],");
           }
           if(_cats != null && _cats.Count > 0)
           {
              sb.Append("Cats:[");
              for(int i = 0; i < Cats.Count; i++)
              {
                  sb.Append(Cats[i].ToString());
              }
              sb.Append("],");
           }
           if(_treasures	 != null && _treasures	.Count > 0)
           {
              sb.Append("Treasures	:[");
              for(int i = 0; i < Treasures	.Count; i++)
              {
                  sb.Append(Treasures	[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum GameErrorCode
    {
        /// <summary>
        /// ID错误
        /// </summary>
        E_ID = 1,
        /// <summary>
        /// Type错误
        /// </summary>
        E_Type = 2,
        /// <summary>
        /// 角色已经存在
        /// </summary>
        E_CE = 3,
        /// <summary>
        /// 角色名已经存在
        /// </summary>
        E_NE = 4,
        /// <summary>
        /// 输入字符串不合法
        /// </summary>
        E_SF = 5,
        /// <summary>
        /// 加载数据失败
        /// </summary>
        E_LD = 6,
        /// <summary>
        /// 上次操作未结束
        /// </summary>
        E_CNC = 7,
        /// <summary>
        /// 权限不够,
        /// </summary>
        E_AN = 8,
        /// <summary>
        /// 参数错误
        /// </summary>
        E_P = 9,
        /// <summary>
        /// 不知道出啥错了
        /// </summary>
        E_Un			 = 10,
        /// <summary>
        /// 登录失败
        /// </summary>
        LoginFailed = 11,
        /// <summary>
        /// 无效的行为
        /// </summary>
        InvalidAction = 12,
        /// <summary>
        /// 服务器忙
        /// </summary>
        ServerBusy		 = 13,
        /// <summary>
        /// 注册失败
        /// </summary>
        RegisterFailed = 14,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 错误数据
    /// </summary>
    public partial class GameErrorData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Code"] = 1,
            ["Msg"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<GameErrorData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private GameErrorCode _code	;
        public GameErrorCode Code	
        {
           get { return _code	;}
           set
           { 
               _code	 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private string _msg;
        public string Msg
        {
           get { return _msg;}
           set
           { 
               _msg = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Code	 = (GameErrorCode)reader.ReadInt32();
                        break;
                    case 2:
                        Msg = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Code	, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               if(Msg != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.String, writer);
                   ProtoWriter.WriteString(Msg, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Code	:" + Code	.ToString());
            sb.Append(",");
            if(Msg != null)
            {
                sb.Append("Msg:" + Msg.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 创建角色成功
    /// </summary>
    public partial class CreateCharacterOK : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Name"] = 2,
            ["Gender"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<CreateCharacterOK, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private string _name;
        public string Name
        {
           get { return _name;}
           set
           { 
               _name = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private Gender _gender	;
        public Gender Gender	
        {
           get { return _gender	;}
           set
           { 
               _gender	 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Name = reader.ReadString();
                        break;
                    case 3:
                        Gender	 = (Gender)reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               if(Name != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.String, writer);
                   ProtoWriter.WriteString(Name, writer);
               }
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Gender	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            if(Name != null)
            {
                sb.Append("Name:" + Name.ToString());
                sb.Append(",");
            }
            sb.Append("Gender	:" + Gender	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class PutObjectRequest : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["ScenePointIndex"] = 2,
            ["SubPointIndex"] = 3,
            ["RoomSectionType"] = 4,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<PutObjectRequest, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _scenepointindex;
        public int ScenePointIndex
        {
           get { return _scenepointindex;}
           set
           { 
               _scenepointindex = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _subpointindex;
        public int SubPointIndex
        {
           get { return _subpointindex;}
           set
           { 
               _subpointindex = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private RoomSectionType _roomsectiontype	;
        public RoomSectionType RoomSectionType	
        {
           get { return _roomsectiontype	;}
           set
           { 
               _roomsectiontype	 = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        ScenePointIndex = reader.ReadInt32();
                        break;
                    case 3:
                        SubPointIndex = reader.ReadInt32();
                        break;
                    case 4:
                        RoomSectionType	 = (RoomSectionType)reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(ScenePointIndex, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(SubPointIndex, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)RoomSectionType	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("ScenePointIndex:" + ScenePointIndex.ToString());
            sb.Append(",");
            sb.Append("SubPointIndex:" + SubPointIndex.ToString());
            sb.Append(",");
            sb.Append("RoomSectionType	:" + RoomSectionType	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class MoveObjectRequest : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["ScenePointIndex"] = 2,
            ["SubPointIndex"] = 3,
            ["RoomSectionType"] = 4,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<MoveObjectRequest, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _scenepointindex;
        public int ScenePointIndex
        {
           get { return _scenepointindex;}
           set
           { 
               _scenepointindex = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _subpointindex;
        public int SubPointIndex
        {
           get { return _subpointindex;}
           set
           { 
               _subpointindex = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private RoomSectionType _roomsectiontype	;
        public RoomSectionType RoomSectionType	
        {
           get { return _roomsectiontype	;}
           set
           { 
               _roomsectiontype	 = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        ScenePointIndex = reader.ReadInt32();
                        break;
                    case 3:
                        SubPointIndex = reader.ReadInt32();
                        break;
                    case 4:
                        RoomSectionType	 = (RoomSectionType)reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(ScenePointIndex, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(SubPointIndex, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)RoomSectionType	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("ScenePointIndex:" + ScenePointIndex.ToString());
            sb.Append(",");
            sb.Append("SubPointIndex:" + SubPointIndex.ToString());
            sb.Append(",");
            sb.Append("RoomSectionType	:" + RoomSectionType	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum EntityName
    {
        Item = 1,
        Furni = 2,
        Food = 3,
        Pic = 4,
        Treasure	 = 5,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum FitType
    {
        Min = 1,
        Max = 2,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class DeleteData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["EntityName"] = 1,
            ["Id"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<DeleteData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private EntityName _entityname	;
        public EntityName EntityName	
        {
           get { return _entityname	;}
           set
           { 
               _entityname	 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        EntityName	 = (EntityName)reader.ReadInt32();
                        break;
                    case 2:
                        Id = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)EntityName	, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("EntityName	:" + EntityName	.ToString());
            sb.Append(",");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class TargetActionMessage : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["TargetID"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<TargetActionMessage, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _targetid	;
        public long TargetID	
        {
           get { return _targetid	;}
           set
           { 
               _targetid	 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        TargetID	 = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(TargetID	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("TargetID	:" + TargetID	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class StringMessage : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Msg"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<StringMessage, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private string _msg	;
        public string Msg	
        {
           get { return _msg	;}
           set
           { 
               _msg	 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Msg	 = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               if(Msg	 != null)
               {
                   ProtoWriter.WriteFieldHeader(1, WireType.String, writer);
                   ProtoWriter.WriteString(Msg	, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if(Msg	 != null)
            {
                sb.Append("Msg	:" + Msg	.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class ShopData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Count"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<ShopData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id		;
        public long Id		
        {
           get { return _id		;}
           set
           { 
               _id		 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _count	;
        public int Count	
        {
           get { return _count	;}
           set
           { 
               _count	 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id		 = reader.ReadInt64();
                        break;
                    case 2:
                        Count	 = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id		, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id		:" + Id		.ToString());
            sb.Append(",");
            sb.Append("Count	:" + Count	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class ShopDatas : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Datas"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<ShopDatas, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private List<ShopData> _datas	;
        public List<ShopData> Datas	
        {
           get
           {
               if(_datas	==null)
               {
                   _datas	 = new List<ShopData>();
               }
               return _datas	;
           }
           private set { _datas	=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempDatas	 = new ShopData();
                        var _tokenDatas	 = ProtoReader.StartSubItem(reader);
                        _tempDatas	.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenDatas	, reader);
                        Datas	.Add(_tempDatas	);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(_datas	 != null && _datas	.Count > 0)
           {
              for(int i = 0; i < Datas	.Count; i++)
              {
                  var temp = Datas	[i];
                  ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenDatas	 = ProtoWriter.StartSubItem(Datas	, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenDatas	, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
           if(_datas	 != null && _datas	.Count > 0)
           {
              sb.Append("Datas	:[");
              for(int i = 0; i < Datas	.Count; i++)
              {
                  sb.Append(Datas	[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class BuyShopData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Count"] = 2,
            ["Imm"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<BuyShopData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _count	;
        public int Count	
        {
           get { return _count	;}
           set
           { 
               _count	 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private bool _imm;
        public bool Imm
        {
           get { return _imm;}
           set
           { 
               _imm = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Count	 = reader.ReadInt32();
                        break;
                    case 3:
                        Imm = reader.ReadBoolean();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count	, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteBoolean(Imm, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("Count	:" + Count	.ToString());
            sb.Append(",");
            sb.Append("Imm:" + Imm.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class MoneyData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Coin"] = 1,
            ["Dollar"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<MoneyData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private int _coin		;
        public int Coin		
        {
           get { return _coin		;}
           set
           { 
               _coin		 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _dollar	;
        public int Dollar	
        {
           get { return _dollar	;}
           set
           { 
               _dollar	 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Coin		 = reader.ReadInt32();
                        break;
                    case 2:
                        Dollar	 = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Coin		, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Dollar	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Coin		:" + Coin		.ToString());
            sb.Append(",");
            sb.Append("Dollar	:" + Dollar	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class EasyIDMessage : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["ScenePointIndex"] = 2,
            ["SubPointIndex"] = 3,
            ["RoomSectionType"] = 4,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<EasyIDMessage, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _scenepointindex;
        public int ScenePointIndex
        {
           get { return _scenepointindex;}
           set
           { 
               _scenepointindex = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _subpointindex;
        public int SubPointIndex
        {
           get { return _subpointindex;}
           set
           { 
               _subpointindex = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private RoomSectionType _roomsectiontype	;
        public RoomSectionType RoomSectionType	
        {
           get { return _roomsectiontype	;}
           set
           { 
               _roomsectiontype	 = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        ScenePointIndex = reader.ReadInt32();
                        break;
                    case 3:
                        SubPointIndex = reader.ReadInt32();
                        break;
                    case 4:
                        RoomSectionType	 = (RoomSectionType)reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(ScenePointIndex, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(SubPointIndex, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)RoomSectionType	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("ScenePointIndex:" + ScenePointIndex.ToString());
            sb.Append(",");
            sb.Append("SubPointIndex:" + SubPointIndex.ToString());
            sb.Append(",");
            sb.Append("RoomSectionType	:" + RoomSectionType	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum RoomSectionType
    {
        In	 = 0,
        Out	 = 1,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum ToyCategory
    {
        Init		 = 0,
        Expand		 = 1,
        Specially	 = 2,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum GuideCategory
    {
        Character	 = 1,
        Treasured	 = 2,
        ChangeScene	 = 3,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class GuideData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Type"] = 1,
            ["Category"] = 2,
            ["Level"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<GuideData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _type				;
        public long Type				
        {
           get { return _type				;}
           set
           { 
               _type				 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private GuideCategory _category	;
        public GuideCategory Category	
        {
           get { return _category	;}
           set
           { 
               _category	 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _level			;
        public int Level			
        {
           get { return _level			;}
           set
           { 
               _level			 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Type				 = reader.ReadInt64();
                        break;
                    case 2:
                        Category	 = (GuideCategory)reader.ReadInt32();
                        break;
                    case 3:
                        Level			 = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type				, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Category	, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Level			, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Type				:" + Type				.ToString());
            sb.Append(",");
            sb.Append("Category	:" + Category	.ToString());
            sb.Append(",");
            sb.Append("Level			:" + Level			.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 邮件数据
    /// </summary>
    public partial class MailData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["SenderID"] = 2,
            ["SenderName"] = 3,
            ["SendTime"] = 4,
            ["ReceiverID"] = 5,
            ["ReceiverName"] = 6,
            ["Title"] = 7,
            ["Content"] = 8,
            ["Type"] = 9,
            ["Status"] = 10,
            ["Awards"] = 11,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<MailData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id				;
        public long Id				
        {
           get { return _id				;}
           set
           { 
               _id				 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _senderid		;
        public long SenderID		
        {
           get { return _senderid		;}
           set
           { 
               _senderid		 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private string _sendername		;
        public string SenderName		
        {
           get { return _sendername		;}
           set
           { 
               _sendername		 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private long _sendtime		;
        public long SendTime		
        {
           get { return _sendtime		;}
           set
           { 
               _sendtime		 = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }

        private long _receiverid		;
        public long ReceiverID		
        {
           get { return _receiverid		;}
           set
           { 
               _receiverid		 = value;
               if(!memberSets.ContainsKey(5))
               { 
                    memberSets[5] = 1;
               } 
               else
               { 
                   memberSets[5]++;
               }
           }
        }

        private string _receivername	;
        public string ReceiverName	
        {
           get { return _receivername	;}
           set
           { 
               _receivername	 = value;
               if(!memberSets.ContainsKey(6))
               { 
                    memberSets[6] = 1;
               } 
               else
               { 
                   memberSets[6]++;
               }
           }
        }

        private string _title			;
        public string Title			
        {
           get { return _title			;}
           set
           { 
               _title			 = value;
               if(!memberSets.ContainsKey(7))
               { 
                    memberSets[7] = 1;
               } 
               else
               { 
                   memberSets[7]++;
               }
           }
        }

        private string _content;
        public string Content
        {
           get { return _content;}
           set
           { 
               _content = value;
               if(!memberSets.ContainsKey(8))
               { 
                    memberSets[8] = 1;
               } 
               else
               { 
                   memberSets[8]++;
               }
           }
        }

        private MailType _type			;
        public MailType Type			
        {
           get { return _type			;}
           set
           { 
               _type			 = value;
               if(!memberSets.ContainsKey(9))
               { 
                    memberSets[9] = 1;
               } 
               else
               { 
                   memberSets[9]++;
               }
           }
        }

        private MailStatus _status			;
        public MailStatus Status			
        {
           get { return _status			;}
           set
           { 
               _status			 = value;
               if(!memberSets.ContainsKey(10))
               { 
                    memberSets[10] = 1;
               } 
               else
               { 
                   memberSets[10]++;
               }
           }
        }

        private List<AwardSpecData> _awards			;
        public List<AwardSpecData> Awards			
        {
           get
           {
               if(_awards			==null)
               {
                   _awards			 = new List<AwardSpecData>();
               }
               return _awards			;
           }
           private set { _awards			=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id				 = reader.ReadInt64();
                        break;
                    case 2:
                        SenderID		 = reader.ReadInt64();
                        break;
                    case 3:
                        SenderName		 = reader.ReadString();
                        break;
                    case 4:
                        SendTime		 = reader.ReadInt64();
                        break;
                    case 5:
                        ReceiverID		 = reader.ReadInt64();
                        break;
                    case 6:
                        ReceiverName	 = reader.ReadString();
                        break;
                    case 7:
                        Title			 = reader.ReadString();
                        break;
                    case 8:
                        Content = reader.ReadString();
                        break;
                    case 9:
                        Type			 = (MailType)reader.ReadInt32();
                        break;
                    case 10:
                        Status			 = (MailStatus)reader.ReadInt32();
                        break;
                    case 11:
                        var _tempAwards			 = new AwardSpecData();
                        var _tokenAwards			 = ProtoReader.StartSubItem(reader);
                        _tempAwards			.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenAwards			, reader);
                        Awards			.Add(_tempAwards			);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id				, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(SenderID		, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               if(SenderName		 != null)
               {
                   ProtoWriter.WriteFieldHeader(3, WireType.String, writer);
                   ProtoWriter.WriteString(SenderName		, writer);
               }
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt64(SendTime		, writer);
           }
           if(memberSets.ContainsKey(5))
           {
               ProtoWriter.WriteFieldHeader(5, WireType.Variant, writer);
               ProtoWriter.WriteInt64(ReceiverID		, writer);
           }
           if(memberSets.ContainsKey(6))
           {
               if(ReceiverName	 != null)
               {
                   ProtoWriter.WriteFieldHeader(6, WireType.String, writer);
                   ProtoWriter.WriteString(ReceiverName	, writer);
               }
           }
           if(memberSets.ContainsKey(7))
           {
               if(Title			 != null)
               {
                   ProtoWriter.WriteFieldHeader(7, WireType.String, writer);
                   ProtoWriter.WriteString(Title			, writer);
               }
           }
           if(memberSets.ContainsKey(8))
           {
               if(Content != null)
               {
                   ProtoWriter.WriteFieldHeader(8, WireType.String, writer);
                   ProtoWriter.WriteString(Content, writer);
               }
           }
           if(memberSets.ContainsKey(9))
           {
               ProtoWriter.WriteFieldHeader(9, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Type			, writer);
           }
           if(memberSets.ContainsKey(10))
           {
               ProtoWriter.WriteFieldHeader(10, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Status			, writer);
           }
           if(_awards			 != null && _awards			.Count > 0)
           {
              for(int i = 0; i < Awards			.Count; i++)
              {
                  var temp = Awards			[i];
                  ProtoWriter.WriteFieldHeader(11, WireType.StartGroup, writer);
                  var _tokenAwards			 = ProtoWriter.StartSubItem(Awards			, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenAwards			, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id				:" + Id				.ToString());
            sb.Append(",");
            sb.Append("SenderID		:" + SenderID		.ToString());
            sb.Append(",");
            if(SenderName		 != null)
            {
                sb.Append("SenderName		:" + SenderName		.ToString());
                sb.Append(",");
            }
            sb.Append("SendTime		:" + SendTime		.ToString());
            sb.Append(",");
            sb.Append("ReceiverID		:" + ReceiverID		.ToString());
            sb.Append(",");
            if(ReceiverName	 != null)
            {
                sb.Append("ReceiverName	:" + ReceiverName	.ToString());
                sb.Append(",");
            }
            if(Title			 != null)
            {
                sb.Append("Title			:" + Title			.ToString());
                sb.Append(",");
            }
            if(Content != null)
            {
                sb.Append("Content:" + Content.ToString());
                sb.Append(",");
            }
            sb.Append("Type			:" + Type			.ToString());
            sb.Append(",");
            sb.Append("Status			:" + Status			.ToString());
            sb.Append(",");
           if(_awards			 != null && _awards			.Count > 0)
           {
              sb.Append("Awards			:[");
              for(int i = 0; i < Awards			.Count; i++)
              {
                  sb.Append(Awards			[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 邮件类型
    /// </summary>
    public enum MailType
    {
        /// <summary>
        /// 普通
        /// </summary>
        Common		 = 0,
        /// <summary>
        /// 公告
        /// </summary>
        Notice		 = 10,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 邮件状态
    /// </summary>
    public enum MailStatus
    {
        New			 = 0,
        Read		 = 1,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 奖励描述
    /// </summary>
    public partial class AwardSpecData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Type"] = 1,
            ["Name"] = 2,
            ["Value"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<AwardSpecData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _type	;
        public long Type	
        {
           get { return _type	;}
           set
           { 
               _type	 = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private string _name	;
        public string Name	
        {
           get { return _name	;}
           set
           { 
               _name	 = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _value	;
        public int Value	
        {
           get { return _value	;}
           set
           { 
               _value	 = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Type	 = reader.ReadInt64();
                        break;
                    case 2:
                        Name	 = reader.ReadString();
                        break;
                    case 3:
                        Value	 = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type	, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               if(Name	 != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.String, writer);
                   ProtoWriter.WriteString(Name	, writer);
               }
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Value	, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Type	:" + Type	.ToString());
            sb.Append(",");
            if(Name	 != null)
            {
                sb.Append("Name	:" + Name	.ToString());
                sb.Append(",");
            }
            sb.Append("Value	:" + Value	.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class MailDatas : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Data"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<MailDatas, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private List<MailData> _data	;
        public List<MailData> Data	
        {
           get
           {
               if(_data	==null)
               {
                   _data	 = new List<MailData>();
               }
               return _data	;
           }
           private set { _data	=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempData	 = new MailData();
                        var _tokenData	 = ProtoReader.StartSubItem(reader);
                        _tempData	.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenData	, reader);
                        Data	.Add(_tempData	);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(_data	 != null && _data	.Count > 0)
           {
              for(int i = 0; i < Data	.Count; i++)
              {
                  var temp = Data	[i];
                  ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenData	 = ProtoWriter.StartSubItem(Data	, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenData	, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
           if(_data	 != null && _data	.Count > 0)
           {
              sb.Append("Data	:[");
              for(int i = 0; i < Data	.Count; i++)
              {
                  sb.Append(Data	[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class SMMessage : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Title"] = 1,
            ["Content"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SMMessage, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private string _title;
        public string Title
        {
           get { return _title;}
           set
           { 
               _title = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private string _content;
        public string Content
        {
           get { return _content;}
           set
           { 
               _content = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Title = reader.ReadString();
                        break;
                    case 2:
                        Content = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               if(Title != null)
               {
                   ProtoWriter.WriteFieldHeader(1, WireType.String, writer);
                   ProtoWriter.WriteString(Title, writer);
               }
           }
           if(memberSets.ContainsKey(2))
           {
               if(Content != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.String, writer);
                   ProtoWriter.WriteString(Content, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if(Title != null)
            {
                sb.Append("Title:" + Title.ToString());
                sb.Append(",");
            }
            if(Content != null)
            {
                sb.Append("Content:" + Content.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class AchievementData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Type"] = 2,
            ["Completed"] = 3,
            ["Count"] = 4,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<AchievementData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _type;
        public long Type
        {
           get { return _type;}
           set
           { 
               _type = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _completed;
        public int Completed
        {
           get { return _completed;}
           set
           { 
               _completed = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private int _count;
        public int Count
        {
           get { return _count;}
           set
           { 
               _count = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Type = reader.ReadInt64();
                        break;
                    case 3:
                        Completed = reader.ReadInt32();
                        break;
                    case 4:
                        Count = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Type, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Completed, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("Type:" + Type.ToString());
            sb.Append(",");
            sb.Append("Completed:" + Completed.ToString());
            sb.Append(",");
            sb.Append("Count:" + Count.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class SignData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Has"] = 1,
            ["Times"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SignData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private bool _has;
        public bool Has
        {
           get { return _has;}
           set
           { 
               _has = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _times;
        public int Times
        {
           get { return _times;}
           set
           { 
               _times = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Has = reader.ReadBoolean();
                        break;
                    case 2:
                        Times = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteBoolean(Has, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Times, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Has:" + Has.ToString());
            sb.Append(",");
            sb.Append("Times:" + Times.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class InviteeData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Coin"] = 1,
            ["Dollar"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<InviteeData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private int _coin;
        public int Coin
        {
           get { return _coin;}
           set
           { 
               _coin = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _dollar;
        public int Dollar
        {
           get { return _dollar;}
           set
           { 
               _dollar = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Coin = reader.ReadInt32();
                        break;
                    case 2:
                        Dollar = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Coin, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Dollar, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Coin:" + Coin.ToString());
            sb.Append(",");
            sb.Append("Dollar:" + Dollar.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class LoginAccount : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Name"] = 2,
            ["Password"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<LoginAccount, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private string _name;
        public string Name
        {
           get { return _name;}
           set
           { 
               _name = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private string _password;
        public string Password
        {
           get { return _password;}
           set
           { 
               _password = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Name = reader.ReadString();
                        break;
                    case 3:
                        Password = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               if(Name != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.String, writer);
                   ProtoWriter.WriteString(Name, writer);
               }
           }
           if(memberSets.ContainsKey(3))
           {
               if(Password != null)
               {
                   ProtoWriter.WriteFieldHeader(3, WireType.String, writer);
                   ProtoWriter.WriteString(Password, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            if(Name != null)
            {
                sb.Append("Name:" + Name.ToString());
                sb.Append(",");
            }
            if(Password != null)
            {
                sb.Append("Password:" + Password.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class SpeakRequest : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Message"] = 1,
            ["Type"] = 2,
            ["ChannelID"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SpeakRequest, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private string _message;
        public string Message
        {
           get { return _message;}
           set
           { 
               _message = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private SpeakType _type;
        public SpeakType Type
        {
           get { return _type;}
           set
           { 
               _type = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private long _channelid;
        public long ChannelID
        {
           get { return _channelid;}
           set
           { 
               _channelid = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Message = reader.ReadString();
                        break;
                    case 2:
                        Type = (SpeakType)reader.ReadInt32();
                        break;
                    case 3:
                        ChannelID = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               if(Message != null)
               {
                   ProtoWriter.WriteFieldHeader(1, WireType.String, writer);
                   ProtoWriter.WriteString(Message, writer);
               }
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Type, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt64(ChannelID, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if(Message != null)
            {
                sb.Append("Message:" + Message.ToString());
                sb.Append(",");
            }
            sb.Append("Type:" + Type.ToString());
            sb.Append(",");
            sb.Append("ChannelID:" + ChannelID.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class CharacterSpecData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Name"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<CharacterSpecData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private string _name;
        public string Name
        {
           get { return _name;}
           set
           { 
               _name = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Name = reader.ReadString();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               if(Name != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.String, writer);
                   ProtoWriter.WriteString(Name, writer);
               }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            if(Name != null)
            {
                sb.Append("Name:" + Name.ToString());
                sb.Append(",");
            }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class SpeakData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Character"] = 1,
            ["Message"] = 2,
            ["Type"] = 3,
            ["ChannelID"] = 4,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SpeakData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private CharacterSpecData _character;
        public CharacterSpecData Character
        {
           get { return _character;}
           set
           { 
               _character = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private string _message;
        public string Message
        {
           get { return _message;}
           set
           { 
               _message = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private SpeakType _type;
        public SpeakType Type
        {
           get { return _type;}
           set
           { 
               _type = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private long _channelid;
        public long ChannelID
        {
           get { return _channelid;}
           set
           { 
               _channelid = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempCharacter = new CharacterSpecData();
                        var _tokenCharacter = ProtoReader.StartSubItem(reader);
                         _tempCharacter.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenCharacter, reader);
                        Character = _tempCharacter;
                        break;
                    case 2:
                        Message = reader.ReadString();
                        break;
                    case 3:
                        Type = (SpeakType)reader.ReadInt32();
                        break;
                    case 4:
                        ChannelID = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               if(Character != null)
               {
                   ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenCharacter = ProtoWriter.StartSubItem(Character, writer);
                  Character.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenCharacter, writer);
               }
           }
           if(memberSets.ContainsKey(2))
           {
               if(Message != null)
               {
                   ProtoWriter.WriteFieldHeader(2, WireType.String, writer);
                   ProtoWriter.WriteString(Message, writer);
               }
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Type, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt64(ChannelID, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if(Character != null)
            {
                sb.Append("Character:" + Character.ToString());
                sb.Append(",");
            }
            if(Message != null)
            {
                sb.Append("Message:" + Message.ToString());
                sb.Append(",");
            }
            sb.Append("Type:" + Type.ToString());
            sb.Append(",");
            sb.Append("ChannelID:" + ChannelID.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum SpeakType
    {
        World = 1,
        Channel = 2,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class ShowLotteryData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Times"] = 1,
            ["BuyTimes"] = 2,
            ["LeftTimes"] = 3,
            ["AdvertTimes"] = 4,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<ShowLotteryData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private int _times;
        public int Times
        {
           get { return _times;}
           set
           { 
               _times = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _buytimes;
        public int BuyTimes
        {
           get { return _buytimes;}
           set
           { 
               _buytimes = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _lefttimes;
        public int LeftTimes
        {
           get { return _lefttimes;}
           set
           { 
               _lefttimes = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private int _adverttimes;
        public int AdvertTimes
        {
           get { return _adverttimes;}
           set
           { 
               _adverttimes = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Times = reader.ReadInt32();
                        break;
                    case 2:
                        BuyTimes = reader.ReadInt32();
                        break;
                    case 3:
                        LeftTimes = reader.ReadInt32();
                        break;
                    case 4:
                        AdvertTimes = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Times, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(BuyTimes, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(LeftTimes, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteInt32(AdvertTimes, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Times:" + Times.ToString());
            sb.Append(",");
            sb.Append("BuyTimes:" + BuyTimes.ToString());
            sb.Append(",");
            sb.Append("LeftTimes:" + LeftTimes.ToString());
            sb.Append(",");
            sb.Append("AdvertTimes:" + AdvertTimes.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class EnterChannelRequest : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Type"] = 1,
            ["ChannelID"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<EnterChannelRequest, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private SpeakType _type;
        public SpeakType Type
        {
           get { return _type;}
           set
           { 
               _type = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _channelid;
        public long ChannelID
        {
           get { return _channelid;}
           set
           { 
               _channelid = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Type = (SpeakType)reader.ReadInt32();
                        break;
                    case 2:
                        ChannelID = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt32((int)Type, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(ChannelID, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Type:" + Type.ToString());
            sb.Append(",");
            sb.Append("ChannelID:" + ChannelID.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class SpeakChannelData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Count"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SpeakChannelData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _count;
        public int Count
        {
           get { return _count;}
           set
           { 
               _count = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Count = reader.ReadInt32();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("Count:" + Count.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class SpeakChannelDatas : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Datas"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SpeakChannelDatas, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private List<SpeakChannelData> _datas;
        public List<SpeakChannelData> Datas
        {
           get
           {
               if(_datas==null)
               {
                   _datas = new List<SpeakChannelData>();
               }
               return _datas;
           }
           private set { _datas=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempDatas = new SpeakChannelData();
                        var _tokenDatas = ProtoReader.StartSubItem(reader);
                        _tempDatas.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenDatas, reader);
                        Datas.Add(_tempDatas);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(_datas != null && _datas.Count > 0)
           {
              for(int i = 0; i < Datas.Count; i++)
              {
                  var temp = Datas[i];
                  ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenDatas = ProtoWriter.StartSubItem(Datas, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenDatas, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
           if(_datas != null && _datas.Count > 0)
           {
              sb.Append("Datas:[");
              for(int i = 0; i < Datas.Count; i++)
              {
                  sb.Append(Datas[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class SpeakChannelData2 : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Count"] = 2,
            ["Speaks"] = 3,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SpeakChannelData2, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _count;
        public int Count
        {
           get { return _count;}
           set
           { 
               _count = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private List<SpeakData> _speaks;
        public List<SpeakData> Speaks
        {
           get
           {
               if(_speaks==null)
               {
                   _speaks = new List<SpeakData>();
               }
               return _speaks;
           }
           private set { _speaks=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Count = reader.ReadInt32();
                        break;
                    case 3:
                        var _tempSpeaks = new SpeakData();
                        var _tokenSpeaks = ProtoReader.StartSubItem(reader);
                        _tempSpeaks.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenSpeaks, reader);
                        Speaks.Add(_tempSpeaks);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Count, writer);
           }
           if(_speaks != null && _speaks.Count > 0)
           {
              for(int i = 0; i < Speaks.Count; i++)
              {
                  var temp = Speaks[i];
                  ProtoWriter.WriteFieldHeader(3, WireType.StartGroup, writer);
                  var _tokenSpeaks = ProtoWriter.StartSubItem(Speaks, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenSpeaks, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("Count:" + Count.ToString());
            sb.Append(",");
           if(_speaks != null && _speaks.Count > 0)
           {
              sb.Append("Speaks:[");
              for(int i = 0; i < Speaks.Count; i++)
              {
                  sb.Append(Speaks[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class SpeakChannelDatas2 : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Datas"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<SpeakChannelDatas2, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private List<SpeakChannelData2> _datas;
        public List<SpeakChannelData2> Datas
        {
           get
           {
               if(_datas==null)
               {
                   _datas = new List<SpeakChannelData2>();
               }
               return _datas;
           }
           private set { _datas=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempDatas = new SpeakChannelData2();
                        var _tokenDatas = ProtoReader.StartSubItem(reader);
                        _tempDatas.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenDatas, reader);
                        Datas.Add(_tempDatas);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(_datas != null && _datas.Count > 0)
           {
              for(int i = 0; i < Datas.Count; i++)
              {
                  var temp = Datas[i];
                  ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenDatas = ProtoWriter.StartSubItem(Datas, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenDatas, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
           if(_datas != null && _datas.Count > 0)
           {
              sb.Append("Datas:[");
              for(int i = 0; i < Datas.Count; i++)
              {
                  sb.Append(Datas[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class RedirectSpeakChannel : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["OldID"] = 1,
            ["NewID"] = 2,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<RedirectSpeakChannel, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _oldid;
        public long OldID
        {
           get { return _oldid;}
           set
           { 
               _oldid = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private long _newid;
        public long NewID
        {
           get { return _newid;}
           set
           { 
               _newid = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        OldID = reader.ReadInt64();
                        break;
                    case 2:
                        NewID = reader.ReadInt64();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(OldID, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt64(NewID, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("OldID:" + OldID.ToString());
            sb.Append(",");
            sb.Append("NewID:" + NewID.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class PayItemData : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Id"] = 1,
            ["Money"] = 2,
            ["Dollar"] = 3,
            ["Double"] = 4,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<PayItemData, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private long _id;
        public long Id
        {
           get { return _id;}
           set
           { 
               _id = value;
               if(!memberSets.ContainsKey(1))
               { 
                    memberSets[1] = 1;
               } 
               else
               { 
                   memberSets[1]++;
               }
           }
        }

        private int _money;
        public int Money
        {
           get { return _money;}
           set
           { 
               _money = value;
               if(!memberSets.ContainsKey(2))
               { 
                    memberSets[2] = 1;
               } 
               else
               { 
                   memberSets[2]++;
               }
           }
        }

        private int _dollar;
        public int Dollar
        {
           get { return _dollar;}
           set
           { 
               _dollar = value;
               if(!memberSets.ContainsKey(3))
               { 
                    memberSets[3] = 1;
               } 
               else
               { 
                   memberSets[3]++;
               }
           }
        }

        private bool _double;
        public bool Double
        {
           get { return _double;}
           set
           { 
               _double = value;
               if(!memberSets.ContainsKey(4))
               { 
                    memberSets[4] = 1;
               } 
               else
               { 
                   memberSets[4]++;
               }
           }
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        Id = reader.ReadInt64();
                        break;
                    case 2:
                        Money = reader.ReadInt32();
                        break;
                    case 3:
                        Dollar = reader.ReadInt32();
                        break;
                    case 4:
                        Double = reader.ReadBoolean();
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(memberSets.ContainsKey(1))
           {
               ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
               ProtoWriter.WriteInt64(Id, writer);
           }
           if(memberSets.ContainsKey(2))
           {
               ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Money, writer);
           }
           if(memberSets.ContainsKey(3))
           {
               ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
               ProtoWriter.WriteInt32(Dollar, writer);
           }
           if(memberSets.ContainsKey(4))
           {
               ProtoWriter.WriteFieldHeader(4, WireType.Variant, writer);
               ProtoWriter.WriteBoolean(Double, writer);
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("Id:" + Id.ToString());
            sb.Append(",");
            sb.Append("Money:" + Money.ToString());
            sb.Append(",");
            sb.Append("Dollar:" + Dollar.ToString());
            sb.Append(",");
            sb.Append("Double:" + Double.ToString());
            sb.Append(",");
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public partial class PayItemDatas : ProtoObject
    {
        private Dictionary<int, int> memberSets = new Dictionary<int, int>();
        private Dictionary<string, int> memberIndexs = new Dictionary<string, int>()
        {
            ["Items"] = 1,
        };
   
        public bool IsSetMember(int memberIndex)
        {
            return memberSets.ContainsKey(memberIndex);
        }
        public bool IsSetMember(Expression<Func<PayItemDatas, object>> expre)
        {
            if (expre == null)
                throw new ArgumentNullException(nameof(expre));

            PropertyInfo propertyInfo = null;
            if (expre.Body is UnaryExpression)
                propertyInfo = (PropertyInfo)((expre.Body as UnaryExpression).Operand as MemberExpression).Member;
            else if (expre.Body is MemberExpression)
                propertyInfo = (PropertyInfo)(expre.Body as MemberExpression).Member;
            else
                throw new ArgumentException();

            if (propertyInfo == null)
                throw new ArgumentException();

            int memberIndex;
            if (!memberIndexs.TryGetValue(propertyInfo.Name, out memberIndex))
                throw new ArgumentException();

            return memberSets.ContainsKey(memberIndex);
        }

        private List<PayItemData> _items;
        public List<PayItemData> Items
        {
           get
           {
               if(_items==null)
               {
                   _items = new List<PayItemData>();
               }
               return _items;
           }
           private set { _items=value;}
        }


        public override void ReadFrom(byte[] bytes, int index, int count)
        {
            using (MemoryStream ms = new MemoryStream(bytes, index, count))
            {
                ReadFrom(ms);
            }
        }

        public override void ReadFrom(Stream stream)
        {
            var reader = new ProtoReader(stream);
            ReadFrom(reader);
            reader.Dispose();
        }

        public void ReadFrom(ProtoReader reader)
        {
            int field;
            while ((field = reader.ReadFieldHeader()) != 0)
            {
                switch (field)
                {
                    case 1:
                        var _tempItems = new PayItemData();
                        var _tokenItems = ProtoReader.StartSubItem(reader);
                        _tempItems.ReadFrom(reader);
                        ProtoReader.EndSubItem(_tokenItems, reader);
                        Items.Add(_tempItems);
                        break;
                    default:
                        reader.SkipField();
                        break;
                }
            }
        }
        public override void  WriteTo(Stream stream)
        {
            var writer = new ProtoWriter(stream);
            WriteTo(writer);
            writer.Close();
        }

        public void WriteTo(ProtoWriter writer)
        {
           if(_items != null && _items.Count > 0)
           {
              for(int i = 0; i < Items.Count; i++)
              {
                  var temp = Items[i];
                  ProtoWriter.WriteFieldHeader(1, WireType.StartGroup, writer);
                  var _tokenItems = ProtoWriter.StartSubItem(Items, writer);
                  temp.WriteTo(writer);
                  ProtoWriter.EndSubItem(_tokenItems, writer);
              }
           }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
           if(_items != null && _items.Count > 0)
           {
              sb.Append("Items:[");
              for(int i = 0; i < Items.Count; i++)
              {
                  sb.Append(Items[i].ToString());
              }
              sb.Append("],");
           }
            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("}");
            return sb.ToString();
        }

    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 服务器端对应方法 客户端Send的Opcode
    /// </summary>
    public enum ServerMethod
    {
        /// <summary>
        /// 提意见			value: AdviseData
        /// </summary>
        Advise				 = 50,
        /// <summary>
        /// 登录 value: AccountData			return: key = 100
        /// </summary>
        Login				 = 100,
        /// <summary>
        /// 创建角色			value: CreateCharacter return: key = 110
        /// </summary>
        CreateCharacter		 = 101,
        /// <summary>
        /// 选择角色			value: IDMessage			return: key = 101
        /// </summary>
        SelectCharacter = 102,
        /// <summary>
        /// 下一步新手引导	value: IDMessage			return: key = 120
        /// </summary>
        NextGuideIndex		 = 103,
        /// <summary>
        /// 跳过新手引导 value: IDMessage return: key = 120
        /// </summary>
        SkipGuide = 104,
        /// <summary>
        /// 放置物体			value: PutObjectRequest		return: key = 730
        /// </summary>
        PlaceObject			 = 250,
        /// <summary>
        /// 回收物品			value: IDMessage			return: key = 731
        /// </summary>
        PickUpObject		 = 251,
        /// <summary>
        /// 删除物品			value: IDCountMessage		return: key = 731
        /// </summary>
        DeleteObject		 = 252,
        /// <summary>
        /// 移动物品			value: MoveObjectRequest	return: key = 733
        /// </summary>
        MoveObject			 = 254,
        /// <summary>
        /// 踢猫	 value:IDMessage return：key = 151
        /// </summary>
        KickCat = 300,
        /// <summary>
        /// 领取奖励			value: IDMessage			return: key = 701
        /// </summary>
        DrawAward			 = 450,
        /// <summary>
        /// 领取奖励			value: IDListMessage		return: key = 701
        /// </summary>
        DrawAwards			 = 451,
        /// <summary>
        /// 获取已购买		value: null					return: key = 850
        /// </summary>
        GetPossess			 = 500,
        /// <summary>
        /// 购买				value: IDCountMessage		return: key = 851
        /// </summary>
        BuyShop				 = 510,
        /// <summary>
        /// 倒卖货币			value: IDCountMessage		return: key = 6
        /// </summary>
        BuyCurrency			 = 511,
        /// <summary>
        /// 购买场景			value: IDMessage			return: key = 750
        /// </summary>
        BuyScene			 = 512,
        /// <summary>
        /// 放置食物			value: EasyIDMessage
        /// </summary>
        EasyBuyFood			 = 520,
        /// <summary>
        /// 加钱				value: MoneyData
        /// </summary>
        AddMoney	 = 600,
        /// <summary>
        /// 填充猫
        /// </summary>
        FullCat				 = 601,
        /// <summary>
        /// 选择默认房间		value: IDMessage
        /// </summary>
        SelectDefaultRoom	 = 650,
        /// <summary>
        /// 获取邮件										return:	key = 900
        /// </summary>
        GetMails			 = 700,
        /// <summary>
        /// 读取邮件			value: IDMessage			return: key = 902
        /// </summary>
        ReadMail			 = 701,
        /// <summary>
        /// 获取邮件奖励		value: IDMessage			return: key = 920
        /// </summary>
        GetMailAward		 = 710,
        /// <summary>
        /// 获取全部邮件奖励								return: key = 921
        /// </summary>
        GetAllMailAward		 = 711,
        /// <summary>
        /// 删除邮件			value: IDMessage			return: key = 910
        /// </summary>
        DeleteMail			 = 720,
        /// <summary>
        /// 删除全部邮件									return: key = 911
        /// </summary>
        DeleteAllMail		 = 721,
        /// <summary>
        /// 每日签到 value: return: key = 940
        /// </summary>
        DailySign			 = 730,
        /// <summary>
        /// 被邀请 value: IDMessage return: key = 950
        /// </summary>
        Invitee = 740,
        /// <summary>
        /// 领取成就奖励		value: IDMessage
        /// </summary>
        DrawAchieve			 = 750,
        /// <summary>
        /// 获取数据 value: null return: key = 970
        /// </summary>
        ShowLottery = 760,
        /// <summary>
        /// 抽奖 value: null return: key = 971
        /// </summary>
        Lottery = 761,
        /// <summary>
        /// 广告增加抽奖次数 value: null return: key = 972
        /// </summary>
        AdvertLottery = 762,
        /// <summary>
        /// 喊话 value: SpeakRequest return: key = 1000
        /// </summary>
        Speak = 800,
        /// <summary>
        /// 进入聊天频道 value: EnterChannelRequest return: key = 1001
        /// </summary>
        EnterSpeakChannel = 801,
        LeaveSpeakChannel = 802,
        /// <summary>
        /// 获取频道信息
        /// </summary>
        ShowSpeakChannelData = 803,
        /// <summary>
        /// 获取频道信息
        /// </summary>
        ShowSpeakChannelData2 = 804,
        /// <summary>
        /// 重定向聊天 value: IDMessage
        /// </summary>
        RedirectSpeakChannel = 805,
        /// <summary>
        /// 获取充值列表 value: PayItemDatas return: key = 1050
        /// </summary>
        GetShopItemDatas = 850,
        /// <summary>
        /// 看广告
        /// </summary>
        Advert				 = 900,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum MainServerMethod
    {
        /// <summary>
        /// 心跳
        /// </summary>
        Heartbeat = 50,
        /// <summary>
        /// 登录
        /// </summary>
        Login = 100,
        /// <summary>
        /// 喊话 value: SpeakData return: key = 150
        /// </summary>
        Speak = 150,
        /// <summary>
        /// 更新聊天频道数据 value: SpeakChannelData
        /// </summary>
        UpdateSpeakChannelData = 151,
        /// <summary>
        /// 获取聊天频道数据 value: SpeakChannelDatas2
        /// </summary>
        ShowSpeakChannelData = 152,
        /// <summary>
        /// 进入聊天
        /// </summary>
        EnterSpeakChannel = 153,
        /// <summary>
        /// 重定向聊天 value: RedirectSpeakChannel
        /// </summary>
        RedirectSpeakChannel = 154,
        /// <summary>
        /// 离开聊天
        /// </summary>
        LeaveSpeakChannel = 155,
        /// <summary>
        /// 更新
        /// </summary>
        UpdateSpeakChannelDatas = 156,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// 客户端调用的方法 对应服务器发送的Opcode
    /// </summary>
    public enum ClientMethod
    {
        UpdateCharacter = 6,
        ShowDisconnect		 = 7,
        /// <summary>
        /// 系统时间		value: SystemTime
        /// </summary>
        SystemTime			 = 8,
        /// <summary>
        /// 错误			value: GameErrorData
        /// </summary>
        GameError			 = 12,
        /// <summary>
        /// 弹窗消息		value: StringMessage
        /// </summary>
        ShowMessage			 = 20,
        /// <summary>
        /// 绑定消息 value: StringMessage
        /// </summary>
        BindMessage = 30,
        /// <summary>
        /// 登陆			value: LoginData
        /// </summary>
        Login				 = 100,
        /// <summary>
        /// 进入游戏		value: EnterGameData
        /// </summary>
        EnterGame			 = 101,
        /// <summary>
        /// 创建角色成功	value: CreateCharacterOK
        /// </summary>
        CreateCharacterOK	 = 110,
        /// <summary>
        /// 设置新手引导	value: GuideData
        /// </summary>
        UpdateGuide			 = 120,
        /// <summary>
        /// 猫进入房间	value: CatData
        /// </summary>
        CatEnterRoom		 = 150,
        /// <summary>
        /// 猫离开房间	value: TargetActionMessage
        /// </summary>
        CatLeaveRoom		 = 151,
        /// <summary>
        /// 删除背包物品	value: DeleteData
        /// </summary>
        DeleteObject		 = 200,
        /// <summary>
        /// 增加道具		value: ItemData
        /// </summary>
        NewItem				 = 250,
        /// <summary>
        /// 更新道具		value: ItemData
        /// </summary>
        UpdateItem			 = 251,
        /// <summary>
        /// 增加家具		value: FurniData
        /// </summary>
        NewFurni			 = 550,
        /// <summary>
        /// 更新家具		value: FurniData
        /// </summary>
        UpdateFurni			 = 551,
        /// <summary>
        /// 更新房间家具	value: FurniData
        /// </summary>
        UpdateRoomFurni		 = 552,
        /// <summary>
        /// 增加食物		value: FoodData
        /// </summary>
        NewFood				 = 650,
        /// <summary>
        /// 更新食物		value: FoodData
        /// </summary>
        UpdateFood			 = 651,
        /// <summary>
        /// 更新房间食物	value: FoodData
        /// </summary>
        UpdateRoomFood		 = 652,
        /// <summary>
        /// 新的奖励		value: AwardData
        /// </summary>
        NewAward			 = 700,
        /// <summary>
        /// 删除奖励		value: IDMessage
        /// </summary>
        DeleteAward			 = 701,
        /// <summary>
        /// 删除奖励		value: IDListMessage
        /// </summary>
        DeleteAwards		 = 702,
        /// <summary>
        /// 增加房间物体	value: FurniData
        /// </summary>
        PlaceObject			 = 730,
        /// <summary>
        /// 移除房间物体	value: TargetActionMessage
        /// </summary>
        RemoveObject		 = 731,
        /// <summary>
        /// 移动房间物体	value: FurniData
        /// </summary>
        MoveObject			 = 733,
        /// <summary>
        /// 添加新的场景	value: RoomData
        /// </summary>
        NewRoom				 = 750,
        /// <summary>
        /// 离开房间		value: IDMessage
        /// </summary>
        LeaveRoom			 = 756,
        /// <summary>
        /// 进入房间		value: EnterRoomData
        /// </summary>
        EnterRoom			 = 757,
        /// <summary>
        /// 增加房间食物	value: FoodData
        /// </summary>
        PlaceFood			 = 800,
        /// <summary>
        /// 移动房间食物	value: FoodData
        /// </summary>
        MoveFood			 = 801,
        /// <summary>
        /// 获取已购买	value: ShopDatas
        /// </summary>
        GetPossessResult	 = 850,
        /// <summary>
        /// 更新已购买	value: ShopData
        /// </summary>
        UpdateProssess		 = 851,
        /// <summary>
        /// 立即使用		value: IDMessage
        /// </summary>
        Immediately			 = 852,
        /// <summary>
        /// 获取图鉴 value: PicDatas
        /// </summary>
        GetPicsResult = 853,
        /// <summary>
        /// 更新图鉴 value: PicData
        /// </summary>
        UpdatePic = 854,
        /// <summary>
        /// 赠送猫礼物 value: PicData
        /// </summary>
        SendCatTreasure = 855,
        /// <summary>
        /// 邮件			value: MailDatas
        /// </summary>
        Mails				 = 900,
        /// <summary>
        /// 新邮件		value: MailData
        /// </summary>
        NewMail				 = 901,
        /// <summary>
        /// 读取邮件		value: IDMessage
        /// </summary>
        ReadMail			 = 902,
        /// <summary>
        /// 移除邮件		value: IDMessage
        /// </summary>
        RemoveMail			 = 910,
        /// <summary>
        /// 移除所有邮件
        /// </summary>
        RemoveAllMail		 = 911,
        /// <summary>
        /// 领取邮件奖励	value: IDMessage
        /// </summary>
        GetMailAward		 = 920,
        /// <summary>
        /// 领取所有邮件奖励
        /// </summary>
        GetAllMailAward		 = 921,
        /// <summary>
        /// 新的成就		value: AchievementDate
        /// </summary>
        NewAchieve			 = 930,
        /// <summary>
        /// 更新成就		value: AchievementDate
        /// </summary>
        UpdateAchieve		 = 931,
        /// <summary>
        /// 更新签到数据 value: SignData
        /// </summary>
        UpdateSignData = 940,
        /// <summary>
        /// 被邀请结果	value: InviteeData
        /// </summary>
        InviteeResult = 950,
        /// <summary>
        /// 屏蔽功能
        /// </summary>
        ShieldFunction = 960,
        /// <summary>
        /// 获取数据结果 value: ShowLotteryData
        /// </summary>
        ShowLotteryResult = 970,
        /// <summary>
        /// 抽奖结果 value: AwardSpecData
        /// </summary>
        LotteryResult = 971,
        /// <summary>
        /// 更新抽奖 value: ShowLotteryData
        /// </summary>
        UpdateLottery = 972,
        /// <summary>
        /// 喊话 value: SpeakData
        /// </summary>
        Speak = 1000,
        /// <summary>
        /// 频道数据 value: SpeakChannelDatas
        /// </summary>
        SpeakChannelDataResult = 1001,
        /// <summary>
        /// 频道数据 value: SpeakChannelDatas2
        /// </summary>
        SpeakChannelDataResult2 = 1002,
        /// <summary>
        /// 充值列表 value: PayItemDatas
        /// </summary>
        GetShopItemDatasResult = 1050,
        /// <summary>
        /// 更新充值项 value: PayItemData
        /// </summary>
        UpdateShopItemData = 1051,
        /// <summary>
        /// 广告收益 value: MoneyData
        /// </summary>
        AdvertResult = 1100,
    }

    /// <summary>
    /// 自动生成代码 请勿修改
    /// </summary>
    public enum MainClientMethod
    {
        /// <summary>
        /// 心跳
        /// </summary>
        Heartbeat = 50,
        /// <summary>
        /// 登录
        /// </summary>
        Login = 100,
        /// <summary>
        /// 喊话 value: SpeakData
        /// </summary>
        Speak = 150,
        /// <summary>
        /// 更新聊天频道数据 value: SpeakChannelData2
        /// </summary>
        UpdateSpeakChannelData = 151,
        /// <summary>
        /// 更新聊天频道数据 value: SpeakChannelDatas2
        /// </summary>
        UpdateSpeakChannelDatas = 152,
    }

}
