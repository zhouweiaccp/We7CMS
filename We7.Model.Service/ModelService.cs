using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Data;

namespace We7.Model.Service
{
    [ServiceContract]
    public interface IModelDataService
    {
        /// <summary>
        /// 保存模型信息
        /// </summary>
        /// <param name="modelName">模型名称</param>
        /// <param name="values">数据值集合</param>
        /// <returns>是否保存成功</returns>
        [OperationContract]
        int AddModel(string modelName, ModelDataFieldCollection values);

        /// <summary>
        /// 更新模型
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateModel(string modelName, ModelDataFieldCollection values);

        /// <summary>
        /// 删除模型
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteModel(string modelName, int id);

        /// <summary>
        /// 根据查询条件查询数据
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable QueryModel(string modelName, Criteria criteria);

        /// <summary>
        /// 根据参数查询数据
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="queryKey"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable QueryModel(string modelName, string queryKey,ModelDataParamColleciton parameters);
    }

    public interface IModelUIService
    {
        /// <summary>
        /// 根据模型名称取得控件集合
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        [OperationContract]
        ModelControlCollection GetEditControls(string modelName);

        /// <summary>
        /// 取得指定面板的录入控件信息
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="panel"></param>
        /// <returns></returns>
        [OperationContract]
        ModelControlCollection GetEditControls(string modelName,string panel);
        
        /// <summary>
        /// 根据模型取得查询控件
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        [OperationContract]
        ModelControlCollection GetQueryControls(string modelName);

        /// <summary>
        /// 取得指定面板的查询控件信息
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        [OperationContract]
        ModelControlCollection GetQueryControls(string modelName,string panel);

        /// <summary>
        /// 根据模型取得列表列
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        [OperationContract]
        ModelColumnCollection GetColumns(string modelName);

        /// <summary>
        /// 取得指定面板的
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        [OperationContract]
        ModelColumnCollection GetColumns(string modelName,string panel);





    }

    [DataContract]
    public class ModelControl
    {
        /// <summary>
        /// 控件ID号
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// 控件标签
        /// </summary>
        [DataMember]
        public string Label { get; set; }

        /// <summary>
        /// 控件类型
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// 控件长度
        /// </summary>
        [DataMember]
        public int Width { get; set; }

        /// <summary>
        /// 控件高度
        /// </summary>
        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// 是否必填项
        /// </summary>
        [DataMember]
        public bool Required { get; set; }

        /// <summary>
        /// 当前控件的数据值
        /// </summary>
        [DataMember]
        public object value { get; set; }

        private ModelParamCollection _params = new ModelParamCollection();
        /// <summary>
        /// 参数集合
        /// </summary>
        [DataMember]
        public ModelParamCollection Params
        {
            get { return _params; }
            set { _params = value; }
        }
    }

    [DataContract]
    public class ModelControlCollection : List<ModelControl>
    {
    }

    [DataContract]
    public class ModelColumn
    {
        /// <summary>
        /// 字段名
        /// </summary>
        [DataMember]
        public string Field { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// 列宽
        /// </summary>
        [DataMember]
        public int Width { get; set; }

        /// <summary>
        /// 列高
        /// </summary>
        [DataMember]
        public int Height { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        [DataMember]
        public object Extra { get; set; }
    }

    [DataContract]
    public class ModelColumnCollection : List<ModelControl>
    {

    }


    /// <summary>
    /// 模型控件参数
    /// </summary>
    [DataContract]
    public class ModelControlParam
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    /// <summary>
    /// 模型控件参数集合
    /// </summary>
    [DataContract]
    public class ModelParamCollection : List<ModelControlParam>
    {
    }


    /// <summary>
    /// 模型数据项目
    /// </summary>
    [DataContract]
    public class ModelDataField
    {
        public ModelDataField(string field, object value)
        {
            Field = field;
            Value = value;
        }

        public ModelDataField(string field)
            : this(field, null)
        {
        }

        public ModelDataField()
        {
        }

        [DataMember]
        public string Field { get; set; }

        [DataMember]
        public Object Value { get; set; }
    }

    /// <summary>
    /// 模型数据项集合
    /// </summary>
    [DataContract]
    public class ModelDataFieldCollection : List<ModelDataField>
    {
        /// <summary>
        /// 根据字段进行的索引
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object this[string field]
        {
            get
            {
                return (from p in this
                        where String.Compare(field, p.Field, true) == 0
                        select p.Value).FirstOrDefault();
            }
            set
            {
                var param = Find(p => String.Compare(field, p.Field, true) == 0);
                if (param != null)
                {
                    param.Value = value;
                }
                else
                {
                    param = new ModelDataField(field, value);
                    Add(param);
                }
            }
        }
    }

    [DataContract]
    public class Criteria
    {
    }

    /// <summary>
    /// 数据参数
    /// </summary>
    [DataContract]
    public class ModelDataParam
    {
        public ModelDataParam(string field, object value)
        {
            Field = field;
            Value = value;
        }

        public ModelDataParam(string field)
            : this(field, null)
        {
        }

        public ModelDataParam()
        {
        }

        public string Field { get; set; }

        public object Value { get; set; }
    }

    /// <summary>
    /// 数据参数集合
    /// </summary>
    [DataContract]
    public class ModelDataParamColleciton : List<ModelDataParam>
    {
        /// <summary>
        /// 根据字段进行的索引
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object this[string field]
        {
            get
            {
                return (from p in this
                        where String.Compare(field, p.Field, true) == 0
                        select p.Value).FirstOrDefault();
            }
            set
            {
                var param = Find(p => String.Compare(field, p.Field, true) == 0);
                if (param != null)
                {
                    param.Value = value;
                }
                else
                {
                    param = new ModelDataParam(field, value);
                    Add(param);
                }
            }
        }
    }
}
