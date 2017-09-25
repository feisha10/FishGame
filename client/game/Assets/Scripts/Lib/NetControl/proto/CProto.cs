using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Client
{
    public partial class CProto
    {
        public class CIdDef
        {
            public int id;
            public Type tos;
            public Type toc;

            public CIdDef(int _id, Type _tos, Type _toc)
            {
                id = _id; tos = _tos; toc = _toc;
            }
        }

        //  目前的包采用了双向加解密和尾部添加两字节记录包内容的ASC码和来进行校验,会根据该包的发送/接受顺序ID(基于前后双端补包机制的ID记录)
        //  累加到校验和之中来进行校验但是并不将该ID放入包内.   非内容包（如ping包，connect包等）的发送/接受顺序ID统一为0
        public enum PType
        {
            none = -1,//表示没有消息或者不是一个完整消息
            connect = 2,
            buff,
            json,
            ping,
            pong,
            specpt,     //特殊包：包实体是个列表，列表的元素类型不相同
            fight       //战斗协议包
        }

        public const int PackHeadSize = 2;
        public const int PackVerifySize = 2;

        public CProto()
        {
            InitIdDic();
        }

        public bool Encoder<T>(Stream sm, T o)
        {
            return Encode(sm, typeof(T), o);
        }

        bool Encode(Stream sm, Type type, object o)
        {
            byte[] buf;
            if (type == typeof(Int32))
            {
                buf = BitConvertToBytes(Convert.ToInt32(o));
                sm.Write(buf, 0, buf.Length);
                return true;
            }
            if (type == typeof(Int16))
            {
                buf = BitConvertToBytes(Convert.ToInt16(o));
                sm.Write(buf, 0, buf.Length);
                return true;
            }
            if (type == typeof(Boolean))
            {
                bool b = Convert.ToBoolean(o);
                sm.WriteByte((byte)(b ? 1 : 0));
                return true;
            }
            if (type == typeof(Int64))
            {
                buf = BitConvertToBytes(Convert.ToInt64(o));
                sm.Write(buf, 0, buf.Length);
                return true;
            }
            if (type == typeof(float))
            {
                buf = BitConvertToBytes(Convert.ToSingle(o));
                sm.Write(buf, 0, buf.Length);
                return true;
            }
            if (type == typeof(byte))
            {
                buf = BitConverter.GetBytes(Convert.ToByte(o));
                sm.Write(buf.ToArray(), 0, buf.Length);
                return true;
            }

            MemoryStream msm = new MemoryStream();

            if (type == typeof(string))
            {
                buf = Encoding.UTF8.GetBytes(Convert.ToString(o));
                msm.Write(buf, 0, buf.Length);
            }
            else if (type.IsArray)
            {
                if (o != null)
                {
                    if (type == typeof(byte[]))
                    {
                        buf = msm.ToArray();
                        Encode(sm, typeof(Int16), buf.Length);
                        sm.Write(buf, 0, buf.Length);
                        buf = o as byte[];
                        sm.Write(buf.ToArray(), 0, buf.Length);
                        return true;
                    }
                    else
                    {
                        Array a = (Array)o;
                        foreach (object av in a)
                        {
                            if (!Encode(msm, type.GetElementType(), av))
                                return false;
                        }
                    }
                }
            }
            else if (o != null)
            {
                if (!EncodeProto(msm, o))
                    return false;
            }

            buf = msm.ToArray();

            Encode(sm, typeof(Int16), buf.Length);
            sm.Write(buf, 0, buf.Length);
            return true;
        }

        bool EncodeProto(Stream sm, object o)
        {
            bool IsNeedGoodDell = false;
            int continueNUM = 0;
            FieldInfo[] fields = o.GetType().GetFields();
            if (o.GetType().Name==("p_goods"))
            {
                IsNeedGoodDell = true;
            }
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];

                object v = f.GetValue(o);
                if (IsNeedGoodDell && v == null && f.FieldType != typeof(string))
                {
                    continueNUM++;
                    continue;
                }
                if (IsNeedGoodDell&&continueNUM > BasePGoods.count)
                {
                    Encode(sm, typeof(Int16), 0);
                }
                if (!Encode(sm, f.FieldType, v))
                    return false;
            }


            return true;
        }

        T Decode<T>(Stream sm)
        {
            return (T)Decode(sm, typeof(T));
        }

        //解析协议变量用的缓存字节数组（避免频繁new对象）
        byte[] _byteBuf = new byte[1];
        byte[] _uint2Buf = new byte[2];
        byte[] _byt4Buf = new byte[4];
        byte[] _int8Buf = new byte[8];
        private byte _tmp;

        object Decode(Stream sm, Type type)
        {
            byte[] buf;
            if (type == typeof(Int32))
            {
                buf = _byt4Buf;
                sm.Read(buf, 0, buf.Length);

                _tmp = buf[0];
                buf[0] = buf[3];
                buf[3] = _tmp;
                _tmp = buf[1];
                buf[1] = buf[2];
                buf[2] = _tmp;

                return BitConverter.ToInt32(buf, 0);
            }
            if (type == typeof(Int16))
            {
                buf = _uint2Buf;
                sm.Read(buf, 0, buf.Length);

                _tmp = buf[0];
                buf[0] = buf[1];
                buf[1] = _tmp;

                return BitConverter.ToInt16(buf, 0);
            }
            if (type == typeof(Boolean))
            {
                return (Boolean)(sm.ReadByte() > 0);
            }
            if (type == typeof(UInt16))
            {
                buf = _uint2Buf;
                sm.Read(buf, 0, buf.Length);

                _tmp = buf[0];
                buf[0] = buf[1];
                buf[1] = _tmp;

                return BitConverter.ToUInt16(buf, 0);
            }
            if (type == typeof(Int64))
            {
                buf = _int8Buf;
                sm.Read(buf, 0, buf.Length);

                _tmp = buf[0];
                buf[0] = buf[7];
                buf[7] = _tmp;
                _tmp = buf[1];
                buf[1] = buf[6];
                buf[6] = _tmp;
                _tmp = buf[2];
                buf[2] = buf[5];
                buf[5] = _tmp;
                _tmp = buf[3];
                buf[3] = buf[4];
                buf[4] = _tmp;

                return BitConverter.ToInt64(buf, 0);
            }
            if (type == typeof(float))
            {
                buf = _byt4Buf;
                sm.Read(buf, 0, buf.Length);

                _tmp = buf[0];
                buf[0] = buf[3];
                buf[3] = _tmp;
                _tmp = buf[1];
                buf[1] = buf[2];
                buf[2] = _tmp;
                return BitConverter.ToSingle(buf, 0);
            }
            if (type == typeof(byte))
            {
                buf = _byteBuf;
                sm.Read(buf, 0, buf.Length);
                return buf[0];
            }

            int size = Decode<UInt16>(sm);

            if (type == typeof(byte[]))
            {
                buf = new byte[size];
                sm.Read(buf, 0, buf.Length);
                return buf;
            }

            buf = new byte[size];
            sm.Read(buf, 0, buf.Length);
            if (type == typeof(string))
            {
                return Encoding.UTF8.GetString(buf, 0, buf.Length);
            }

            MemoryStream msm = new MemoryStream(buf);
            if (type.IsArray)
            {
                List<object> list = new List<object>();
                while (msm.Position < msm.Length)
                {
                    object v = Decode(msm, type.GetElementType());
                    list.Add(v);
                }
                Array a;

#if UNITY_WEBPLAYER || UNITY_WP8
                ConstructorInfo ci = type.GetConstructor(new Type[] { typeof(int) });
                a = (Array)ci.Invoke(new object[] { list.Count });
#else
                object obj = type.Assembly.CreateInstance(type.FullName, true, BindingFlags.Default, null, new object[] { list.Count }, null, null);
                a = (Array)obj;
#endif
                int i = 0;
                foreach (object v in list)
                    a.SetValue(v, i++);
                return a;
            }
           else if (type == typeof(p_msg))
           {
               return DecodePmsg(msm);
           }
           else if (type == typeof(p_letter_msg))
           {
               return DecodePLmsg(msm);
           }
           else if (type == typeof(p_title_msg))
           {
               return DecodePTmsg(msm);
           }
            else
            {
                if (size == 0)
                    return null;

                return DecodeProto(msm, type);
            }
        }

        object DecodeProto(Stream sm, Type type)
        {
            object o = type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            FieldInfo[] fields = type.GetFields();

            int len = fields.Length;
            int i = 0;
            if (o is Client.p_goods)
            {
                len = len - BasePGoods.count;
                while (i < len)
                {
                    if (i == 8)//ext_info
                    {
                        FieldInfo fi = fields[BasePGoods.get_index((o as p_goods).type)];
                        object goodsV = Decode(sm, fi.FieldType);
                        fi.SetValue(o, goodsV);
                    }
                    else
                    {
						int n = i;
						if (i > 8)
							n = i + BasePGoods.count;

                        FieldInfo f = fields[n];
                        object v = Decode(sm, f.FieldType);
                        f.SetValue(o, v);
                    }

                    i++;
                }
            }
            else //非p_goods走原来方法
            {
                while (i < len)
                {
                    FieldInfo f = fields[i];
                    object v = Decode(sm, f.FieldType);
                    f.SetValue(o, v);

                    i++;
                }
            }
            return o;
        }

        object DecodeProtoSP(Stream sm, Type type)
        {
            object o = type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];

                if (is_base_type(f.FieldType)) //如果前面有字段，正常解析
                {
                    object v = Decode(sm, f.FieldType);
                    f.SetValue(o, v);
                }
                else
                {
                    //如果是复合结构，约定是最后一个字段，根据字段位置解析
                    FieldInfo[] fs = f.FieldType.GetFields();
                    Dictionary<string, object> kvs = new Dictionary<string, object>();
                    int key = 0;
                    while (sm.Position < sm.Length)
                    {
                        key = sm.ReadByte() - 2;
                        FieldInfo field = fs[key];
                        object v = Decode(sm, field.FieldType);
                        if (kvs.ContainsKey(field.Name))
                        {
                            Log.Error("重复的ProtocKey:" + field.Name);
                        }
                        else
                        {
                            kvs.Add(field.Name, v);
                        }

                    }
                    fields[fields.Length - 1].SetValue(o, kvs); //最后一个字段作为容器,此字段是生成的，定义文件不需要有
                    break;
                }
            }

            return o;
        }

        //解析格式化的消息
       object DecodePmsg(Stream sm)
       {
           int msgid = decodeInt32(sm);
           //   Log.Debug(msgid);
           var list = _decodePmsg(sm);

       /*    string msg = string.Format(Lang.GetMessage(msgid), list.ToArray());
           ConfigLangTxt configLang = Lang.GetConfigLang(msgid);
           if (string.IsNullOrEmpty(msg) == false && configLang.msgType == 0)
           {
               Color color = configLang.isFriendly ? ColorEnum.White : ColorEnum.Red;
               TipManager.Instance.ShowTip(msg, color, false,1);
           }*/

           p_msg pMsg = new p_msg();
           pMsg.id = msgid;
           //pMsg.msg = msg;
           return pMsg;
       }

        //解析格式化的消息
       object DecodePLmsg(Stream sm)
       {
           int msgid = decodeInt32(sm);
           var list = _decodePmsg(sm);

         //  string msg = string.Format(Lang.GetMessage(msgid), list.ToArray());

           p_letter_msg pMsg = new p_letter_msg();
           pMsg.id = msgid;
           //pMsg.msg = msg;
           return pMsg;
       }

        //解析格式化的消息
       object DecodePTmsg(Stream sm)
       {
           int msgid = decodeInt32(sm);
           var list = _decodePmsg(sm);

          /* ConfigTitle title = ConfigManager.Instance.GetConfig<ConfigTitle>(Mathf.Abs(msgid));
           string msg = "";
           if (title != null && list != null)
               msg = string.Format(title.name, list.ToArray());
           else if (title == null)
               Log.Error("title表找不到id为" + (Mathf.Abs(msgid)) + "的配置");*/
           p_title_msg pMsg = new p_title_msg();
           pMsg.id = msgid;
           //pMsg.msg = msg;
           return pMsg;
       }

       public string GetDecodePmsg(string str)
       {
           byte[] sms = Encoding.UTF8.GetBytes(str);
           MemoryStream sm = new MemoryStream(sms);
           int msgid = decodeInt32(sm);
           //   Log.Debug(msgid);
           var list = _decodePmsg(sm);
            // string msg = string.Format(Lang.GetMessage(msgid), list.ToArray());
            // return msg;
            return "";
       }

        
        public static p_msg EncodePmsg(int id,byte[] type,string[] infos)
        {
            p_msg back=new p_msg();
            back.id = id;
            List<byte> eninfos= new List<byte>();
            if(type!=null)
            {
                for (int i = 0; i < infos.Length; i++)
                {
                    eninfos.AddRange(_encodePmsg(type[i], infos[i]));
                }
            }

            back.msg = Encoding.UTF8.GetString(eninfos.ToArray());
            return back;
        }


        private static List<byte> _encodePmsg(byte type, string info)
        {
            List<byte> backBytes=new List<byte>();
            backBytes.Add(type);
            switch (type)
            {
                case 1: //字符串
                    byte[] bytes = Encoding.UTF8.GetBytes(info);
                    Int16 length = (Int16)bytes.Length;
                    byte[] lengthByte = GetByteFromInt16(length);
                    backBytes.AddRange(lengthByte);
                    backBytes.AddRange(bytes);
                    break;
                default:
                    return null; //如果到这里，应该是出错，果断停止
            }
            return backBytes;
        }

        private static byte[] GetByteFromInt16(Int16 par)
        {
            byte[] bytes=new byte[2];
            bytes[1] = (byte) (par & 0xff);
            bytes[0] = (byte) ((par & 0xff00)>>8);
            return bytes;
        }
        private static byte[] GetByteFromInt32(int par)
        {
            Log.Debug("par" + par);
            byte[] bytes = new byte[4];
            bytes[3] = (byte)(par & 0xff);
            bytes[2] = (byte)((par & 0xff00) >> 8);
            bytes[1] = (byte)((par & 0xff0000) >> 16);
            bytes[0] = (byte)((par>>24 )& 0xff);
            return bytes;
        }
       private List<string> _decodePmsg(Stream sm)
       {
           List<string> list = new List<string>();
           while (sm.Position < sm.Length)
           {
               int num = sm.ReadByte();
               switch (num)
               {
                   case 2: //item
                       int typeId = decodeInt32(sm);
                       break;
                   case 3: //num
                       list.Add(decodeInt32(sm).ToString()); break;
                       break;
                   case 4: //role
                       long roleID = decodeInt64(sm);
                       string name = decodeString(sm);
                       list.Add(name);
                       break;
                   case 5: //嵌套列表
                       int size = Decode<UInt16>(sm);
                       byte[] buf;
                       buf = new byte[size];
                       sm.Read(buf, 0, buf.Length);
                       MemoryStream msm = new MemoryStream(buf);
                       List<string> tmpList = _decodePmsg(msm);
                       string tmpStr = "";
                       foreach (var one in tmpList)
                       {
                           tmpStr += one + '、';
                       }
                       tmpStr = tmpStr.TrimEnd('、');
                       list.Add(tmpStr);
                       break;
                   case 6://带数量的道具
                       int itemTypeId = decodeInt32(sm);
                       int itemNum = decodeInt32(sm);
                       //只有金币
                       
                       break;
                   case 1: //字符串
                       list.Add(decodeString(sm));
                       break;
                    case 7://嵌套msg_id
                        int msgId = decodeInt32(sm);
                        break;
                    case 8:
                        int charId = decodeInt32(sm);
                        break;
                    case 9:
                        int skinId = decodeInt32(sm);
                        break;
                    case 10:
                        break;
                    default:
                       return null; //如果到这里，应该是出错，果断停止
               }
           }
           return list;
       }

        /// <summary>
        /// 将一个消息tos对象编码成字节数组
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public byte[] ProtoToBytes(BaseToSC o, int sendID)
        {
            MemoryStream msm = new MemoryStream();
            msm.WriteByte((byte)PType.buff);
            Encode(msm, typeof(Int16), o.__ID());
            if (!EncodeProto(msm, o))
                return null;
            NetAppLayer.Instance.StartCheckSendTos(o.__ID());
            return WrapperBytes(msm.ToArray(), sendID);
        }

        /// <summary>
        /// 包装字节数组，数组最前面加上两位字节用来标识信息流长度
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public byte[] WrapperBytes(byte[] buffer, int sendID = 0)
        {
            int len = buffer.Length + PackVerifySize;
            byte[] sendBuffer = new byte[PackHeadSize + len];
            for (int i = PackHeadSize - 1; i >= 0; i--)
            {
                sendBuffer[i] = (byte)len;
                len >>= 8;
            }
            buffer.CopyTo(sendBuffer, PackHeadSize + PackVerifySize);
            return enc(sendBuffer, sendID);
        }

        /// <summary>
        /// 解析出字节流中的消息部分的字节长度，没有解析到或解析全则返回-1
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="recvLen"></param>
        /// <returns></returns>
        public int UnWrapperBytes(byte[] buffer, int recvLen, int recvPackID = 0)
        {
            if (buffer == null || recvLen <= 2 || buffer.Length < recvLen)
                return -1;

            int size = 0;
            for (int i = 0; i < PackHeadSize; i++)
                size = (size << 8) + buffer[i];
            if (recvLen < PackHeadSize + size)
                return -1;

            buffer = dec(buffer, recvPackID);
            return size;
        }

        /// <summary>
        /// 从字节流中解析出消息类型和消息接口toc
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="recvLen"></param>
        /// <param name="toc"></param>
        /// <returns>返回消息的类型</returns>
        public CProto.PType BytesToProto(byte[] buffer, ref int recvLen, out object toc, int recvPackID)
        {
            CProto.PType type = PType.none;

            int size = UnWrapperBytes(buffer, recvLen, recvPackID);
            if (size < 0)
            {
                toc = null;
                return type;
            }

            byte[] bytes = new byte[size - PackVerifySize];
            Array.Copy(buffer, PackHeadSize, bytes, 0, size - PackVerifySize);

            recvLen -= (PackHeadSize + size);
            for (int i = 0; i < recvLen; i++)
                buffer[i] = buffer[i + PackHeadSize + size];

            MemoryStream ms = new MemoryStream(bytes);
            type = (PType)ms.ReadByte();
            switch (type)
            {
                case PType.buff:
                    int id = Decode<Int16>(ms);
                    if(m_idDic.ContainsKey(id))
                    {
                        toc = DecodeProto(ms, m_idDic[id]);
                    }
                    else
                    {
                        toc = null;
                    }
                    break;
                case PType.pong://如有需要，可以想办法通知应用层
                    toc = null;
                    break;
                case PType.specpt://特殊包
                    int id2 = Decode<Int16>(ms);
                    toc = DecodeProtoSP(ms, m_idDic[id2]);
                    break;
                case PType.fight://战斗包
                    var t = new m_fight_udp_pack_toc();
                    t.data = new byte[bytes.Length - 1];
                    ms.Read(t.data, 0, t.data.Length);
                    toc = t;
                    break;
                default:
                    toc = null;
                    break;
            }
            return type;
        }

        public int decodeInt32(Stream sm)
        {
            byte[] buf = new byte[4];
            sm.Read(buf, 0, buf.Length);

            _tmp = buf[0];
            buf[0] = buf[3];
            buf[3] = _tmp;
            _tmp = buf[1];
            buf[1] = buf[2];
            buf[2] = _tmp;

            return BitConverter.ToInt32(buf, 0);
        }

        public long decodeInt64(Stream sm)
        {
            byte[] buf = new byte[8];
            sm.Read(buf, 0, buf.Length);

            _tmp = buf[0];
            buf[0] = buf[7];
            buf[7] = _tmp;
            _tmp = buf[1];
            buf[1] = buf[6];
            buf[6] = _tmp;
            _tmp = buf[2];
            buf[2] = buf[5];
            buf[5] = _tmp;
            _tmp = buf[3];
            buf[3] = buf[4];
            buf[4] = _tmp;

            return BitConverter.ToInt64(buf, 0);
        }

        public static int BytesToInt32(byte[] bytes, int start = 0)
        {
            return (bytes[start] << 24) + (bytes[start + 1] << 16) + (bytes[start + 2] << 8) + bytes[start + 3];
        }

        public static int BytesToInt16(byte[] bytes, int start = 0)
        {
            return (bytes[start] << 8) + bytes[start + 1];
        }

        /// <summary>
        /// 转成大端形式的字节
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static byte[] BitConvertToBytes(float val)
        {
            byte[] bs = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
                return bs.Reverse().ToArray();
            return bs;
        }
        public static byte[] BitConvertToBytes(double val)
        {
            byte[] bs = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
                return bs.Reverse().ToArray();
            return bs;
        }
        public static byte[] BitConvertToBytes(long val)
        {
            byte[] bs = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
                return bs.Reverse().ToArray();
            return bs;
        }

        public static byte[] BitConvertToBytes(int val)
        {
            byte[] bs = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
                return bs.Reverse().ToArray();
            return bs;
        }

        public static byte[] BitConvertToBytes(short val)
        {
            byte[] bs = BitConverter.GetBytes(val);
            if (BitConverter.IsLittleEndian)
                return bs.Reverse().ToArray();
            return bs;
        }

        public string decodeString(Stream sm)
        {
            int size = Decode<Int16>(sm);
            byte[] buf = new byte[size];
            sm.Read(buf, 0, buf.Length);
            return Encoding.UTF8.GetString(buf, 0, buf.Length);
        }

        public static byte[] enc(byte[] bytes, int sendID)
        {
            int Len = bytes.Length;
            int verifyCode = (int)((long)sendID * sendID % 100000);
            int offset = PackHeadSize + PackVerifySize;
            for (int i = offset; i < Len; i++)
            {
                verifyCode += bytes[i];
                bytes[i] = (byte)(((int)bytes[i] + get_mask(sendID + i - offset)) % 256);
            }
            bytes[PackHeadSize] = (byte)((verifyCode % 65536) >> 8);
            bytes[PackHeadSize + 1] = (byte)(verifyCode % 256);
            return bytes;
        }

        //对前两个"长度"字节不做加解密(因为服务器方面是Erlang底层tcp系统自带的封装),然后对这两个字节标示的后续长度的内容进行解密处理,
        //对长度字节后的数据则不进行处理,因为bytes是65536个字节,后面的数据是需要用到的,就不能做处理.后面的数据是待处理的数据内容.
        public static byte[] dec(byte[] bytes, int code)
        {
            int PackLen = (bytes[0] << 8) + bytes[1];
            int verifyCode = bytes[PackLen - 2 + PackVerifySize] * 256 + bytes[PackLen - 1 + PackVerifySize];
            bool needVerify = code != 0;
            for (int i = 2; i < PackLen + 2 - PackVerifySize; i++)
            {
                bytes[i] = (byte)(((int)bytes[i] - get_mask(i - 2)) % 256);
                if (needVerify)
                    code += bytes[i];
            }
            if (needVerify && (bytes[2] != (byte)PType.pong && bytes[2] != (byte)PType.ping))
                bytes[2] = ((code % 65536) == verifyCode) ? bytes[2] : (byte)100; //ptype为100即校验失败，不予处理
            return bytes;
        }

        public static bool is_base_type(Type type)
        {
            return type == typeof(Int32) || type == typeof(string) || type == typeof(Boolean) || type == typeof(Int16) || type == typeof(Int64) ||
                 type == typeof(float) || type == typeof(p_msg);
        }

        private static int[] _MaskTable = {43,74,29,100,117,103,18,42,66,36,113,26,117,15,87,107,82,
	             106,13,112,95,23,11,67,13,18,114,44,82,85,75,71,10,24,75,6,
	             88,10,72,49,58,6,62,104,30,113,52,117,32,41,96,9,49,128,7,
	             55,116,53,100,6,28,75,122,62,13,13,41,97,100,77,44,106,46,
	             32,74,45,113,83,122,108,69,10,60,4,16,53,123,21,57,81,9,59,
	             124,63,14,101,43,23,58,100,29,67,118,93,109,123,124,5,99,
	             101,17,36,88,88,63,39,65,18,118,4,72,57,71,69,61,50,75,102};

        static int get_mask(int index)
        {
            //            int[] MaskTable = {43,74,29,100,117,103,18,42,66,36,113,26,117,15,87,107,82,
            //	             106,13,112,95,23,11,67,13,18,114,44,82,85,75,71,10,24,75,6,
            //	             88,10,72,49,58,6,62,104,30,113,52,117,32,41,96,9,49,128,7,
            //	             55,116,53,100,6,28,75,122,62,13,13,41,97,100,77,44,106,46,
            //	             32,74,45,113,83,122,108,69,10,60,4,16,53,123,21,57,81,9,59,
            //	             124,63,14,101,43,23,58,100,29,67,118,93,109,123,124,5,99,
            //	             101,17,36,88,88,63,39,65,18,118,4,72,57,71,69,61,50,75,102};
            //int i = (index - 1) % 128 + 1;
            return _MaskTable[index % 128];
        }
    }
}
