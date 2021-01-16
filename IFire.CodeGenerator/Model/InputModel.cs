using System.ComponentModel.DataAnnotations;

namespace IFire.CodeGenerator.Model {

    public class InputModel {

        /// <summary>
        /// 数据库表名
        /// </summary>
        [Required]
        public string TableName { get; set; }

        /// <summary>
        /// 类名前缀(默认=TableName)
        /// </summary>
        public string ClassPrefix { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 是否创建Entity
        /// </summary>
        public bool CreateEntity { get; set; } = true;

        /// <summary>
        /// 创建Cotroller
        /// </summary>
        public bool CreateCotroller { get; set; } = true;

        /// <summary>
        /// 创建Service
        /// </summary>
        public bool CreateService { get; set; } = true;

        /// <summary>
        /// 创建DTO
        /// </summary>
        public bool CreateDto { get; set; } = true;
    }
}
