using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiemensWebAPI.Helpers
{
    public enum UserTypes
    {
        ADMIN,
        OPERATOR,
        ENGINEER
    }
    
    public enum PermissionsEnum
    {
        CAN_VIEW_PRODUCT_STOCK,
        CAN_VIEW_STORAGE,
        CAN_VIEW_FEEDSTOCK,
        CAN_DO_ORDERS,
        CAN_RESSUPLY,
        CAN_EDIT_FEEDSTOCK,
        CAN_EDIT_STORAGE,
        CAN_EDIT_PRODUCT_STOCK,
        CAN_EDIT_ORDERS,
        CAN_VIEW_STOCK_GRAPH,
        CAN_VIEW_SOLD_PRODUCTS_GRAPH,
        CAN_CREATE_USERS,
        CAN_EDIT_USER_RIGHTS
    }

    public class UserManagementHelper
    {
        private static PermissionsEnum[] GetOperatorPermissions()
        {
            PermissionsEnum[] operatorRights = { PermissionsEnum.CAN_VIEW_FEEDSTOCK,
                                    PermissionsEnum.CAN_VIEW_PRODUCT_STOCK,
                                    PermissionsEnum.CAN_VIEW_STORAGE,
                                    PermissionsEnum.CAN_DO_ORDERS,
                                    PermissionsEnum.CAN_RESSUPLY };
            return operatorRights;
        }

        private static PermissionsEnum[] GetEngineerPermissions()
        {
            PermissionsEnum[] engineerRights = { PermissionsEnum.CAN_VIEW_FEEDSTOCK,
                                    PermissionsEnum.CAN_EDIT_FEEDSTOCK,
                                    PermissionsEnum.CAN_VIEW_PRODUCT_STOCK,
                                    PermissionsEnum.CAN_EDIT_PRODUCT_STOCK,
                                    PermissionsEnum.CAN_VIEW_STORAGE,
                                    PermissionsEnum.CAN_EDIT_STORAGE,
                                    PermissionsEnum.CAN_VIEW_SOLD_PRODUCTS_GRAPH,
                                    PermissionsEnum.CAN_VIEW_STOCK_GRAPH,
                                    PermissionsEnum.CAN_DO_ORDERS,
                                    PermissionsEnum.CAN_RESSUPLY };
            return engineerRights;
        }

        private static PermissionsEnum[] GetAdminRights()
        {
            PermissionsEnum[] adminRights = { PermissionsEnum.CAN_EDIT_USER_RIGHTS, PermissionsEnum.CAN_CREATE_USERS };
            return adminRights;
        }

        public static Dictionary<string, PermissionsEnum[]> GetPermissionsDictionaryFor(string userRole)
        {
            try
            {
                Dictionary<string, PermissionsEnum[]> permissionsDictionary = new Dictionary<string, PermissionsEnum[]>();
                bool userExists = Enum.IsDefined(typeof(UserTypes), userRole);
                if(userExists)
                {
                    PermissionsEnum[] userPermissions;
                    switch (userRole) { 
                        case "ADMIN":
                            userPermissions = GetAdminRights();
                            break;
                        case "OPERATOR":
                            userPermissions = null;
                            userPermissions = GetOperatorPermissions();
                            break;
                        case "ENGINEER":
                            userPermissions = null;
                            userPermissions = GetEngineerPermissions();
                            break;
                        default:
                            userPermissions = null;
                            break;
                    }
                    permissionsDictionary.Add(userRole, userPermissions);
                }
                return permissionsDictionary;
            }
            catch(ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}