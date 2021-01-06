using System.Collections.Generic;
using System.Linq;
using IFire.Framework.Abstractions;
using IFire.Framework.Extensions;
using IFire.Framework.Helpers;
using IFire.Framework.Interfaces;
using IFire.Framework.Result;
using IFire.WebHost.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace IFire.Auth.Web {

    /// <summary>
    /// 权限集合
    /// </summary>
    public class PermissionCollection : CollectionAbstract<PermissionDescriptor> {
        private readonly MvcHelper _mvcHelper;
        private readonly IConfigProvider _configProvider;
        private TreeResultModel<string, PermissionTreeModel> _tree;

        public PermissionCollection(MvcHelper mvcHelper, IConfigProvider configProvider) {
            _mvcHelper = mvcHelper;
            _configProvider = configProvider;

            LoadPermissions();

            LoadTree();
        }

        /// <summary>
        /// 权限树
        /// </summary>
        public TreeResultModel<string, PermissionTreeModel> Tree {
            get {
                if (_tree == null)
                    LoadTree();

                return _tree;
            }
        }

        /// <summary>
        /// 查询指定控制器的权限列表
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <returns></returns>
        public IList<PermissionDescriptor> Query(string controller) {
            return Collection.Where(m => m.Controller.EqualsIgnoreCase(controller)).ToList();
        }

        /// <summary>
        /// 查询指定权限编码的权限列表
        /// </summary>
        /// <param name="codes">权限编码集合</param>
        /// <returns></returns>
        public IList<PermissionDescriptor> Query(List<string> codes) {
            if (codes == null || !codes.Any())
                return null;

            return Collection.Where(m => codes.Any(n => n.EqualsIgnoreCase(m.Code))).ToList();
        }

        /// <summary>
        /// 加载权限树
        /// </summary>
        /// <returns></returns>
        private void LoadTree() {
            var config = _configProvider.Get<SystemConfig>();
            _tree = new TreeResultModel<string, PermissionTreeModel> {
                Id = "-1",
                Label = config.Title,
                Item = new PermissionTreeModel()
            };
            _tree.Path.Add(_tree.Label);

            var controllers = _mvcHelper.GetAllController();
            //控制器
            foreach (var controller in controllers) {
                var controllerName = controller.Description ?? controller.Name;
                var controllerNode = new TreeResultModel<string, PermissionTreeModel> {
                    Id = $"{controller.Name}".ToLower(),
                    Label = controllerName,
                    Item = new PermissionTreeModel {
                        Label = controllerName
                    }
                };

                controllerNode.Item.Code = controllerNode.Id;

                controllerNode.Path.AddRange(_tree.Path);
                controllerNode.Path.Add(controllerName);

                var permissions = Query(controller.Name);
                //权限
                foreach (var permission in permissions) {
                    var permissionName = permission.Name.Contains("_") ? permission.Name.Split('_')[1] : permission.Name;
                    var permissionNode = new TreeResultModel<string, PermissionTreeModel> {
                        Id = permission.Code,
                        Label = permissionName,
                        Item = new PermissionTreeModel {
                            Label = permissionName,
                            Code = permission.Code,
                            IsPermission = true
                        }
                    };

                    permissionNode.Path.AddRange(controllerNode.Path);
                    permissionNode.Path.Add(permissionName);

                    controllerNode.Children.Add(permissionNode);
                }

                _tree.Children.Add(controllerNode);
            }
        }

        /// <summary>
        /// 加载权限
        /// </summary>
        private void LoadPermissions() {
            var actions = _mvcHelper.GetAllAction();

            foreach (var action in actions) {
                //如果控制器未继承IFireControllerBase抽象类，则表示不需要权限验证
                if (!typeof(IFireControllerBase).IsAssignableFrom(action.Controller.TypeInfo))
                    continue;

                //排除匿名接口和通用接口
                if (action.MethodInfo.CustomAttributes.Any(m => m.AttributeType == typeof(AllowAnonymousAttribute) || m.AttributeType == typeof(CommonAttribute)))
                    continue;

                var p = new PermissionDescriptor {
                    Name = action.Description ?? action.Name,
                    Controller = action.Controller.Name,
                    Action = action.Name
                };

                var httpMethodAttr = action.MethodInfo.CustomAttributes.FirstOrDefault(m => m.AttributeType.Name.StartsWith("Http"));

                if (httpMethodAttr != null) {
                    var httpMethodName = httpMethodAttr.AttributeType.Name.Replace("Http", "").Replace("Attribute", "").ToUpper();
                    p.HttpMethod = httpMethodName;

                    Add(p);
                }
            }
        }
    }
}
