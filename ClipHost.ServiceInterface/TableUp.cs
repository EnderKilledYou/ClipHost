﻿using ClipHost.ServiceModel;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClipHost.ServiceInterface
{
    public static class TableUp
    {
        public static IEnumerable<ITableUp> GetTypesWithAttribute(  Type attributeType)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(x => typeof(ITableUp).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(a => (ITableUp)Activator.CreateInstance(a)));
 
        }
        public static void DoAllTableUps(IDbConnection db)
        {
            
            foreach(var type in GetTypesWithAttribute(typeof(TableUpAttribute)).OrderBy((type => (type.GetType().GetCustomAttribute<TableUpAttribute>()).Order)))
            {

          
                type.TableUp(db.CreateTableIfNotExists);
            }
        }

      
    }
}
