
#region Copyright
//
// Copyright 2024 Gurpreet Raju
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShineGuacamole.DataAccess.SqlServer.Models;
using ShineGuacamole.Library.DataAccess;

namespace ShineGuacamole.DataAccess.SqlServer
{
    /// <summary>
    /// Provides extension methods for start up configuration.
    /// </summary>
    public static class StartupConfiguration
    {
        /// <summary>
        /// Adds the necessary services for the SQL Server.
        /// </summary>
        /// <param name="services"></param>
        public static void AddSqlServerDataAccess(this IServiceCollection services, string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            services.AddScoped<IConnectionsDataAccess, ConnectionDataAccess>();
            services.AddDbContext<ShineGuacContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
