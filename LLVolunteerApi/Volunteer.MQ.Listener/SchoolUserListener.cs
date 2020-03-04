using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMqBus;
using Volunteer.Core.Entities.QueryModel;
using Volunteer.Core.Interfaces;

namespace Volunteer.MQ.Listener
{
    /// <summary>
    /// 用于 监听 身份认证 成功后的消息
    /// </summary>
    public class SchoolUserListener:RabbitListener
    {
        private readonly ILogger _logger;

        // 因为Process函数是委托回调,直接将其他Service注入的话两者不在一个scope,
        // 这里要调用其他的Service实例只能用IServiceProvider CreateScope后获取实例对象
        private readonly IServiceProvider _services;

        public SchoolUserListener(IServiceProvider services,
            ILogger<SchoolUserListener> logger)
        {
            base.RouteKey = "school";
            base.QueueName = "schoolUserQueue";
            _logger = logger;
            _services = services;

        }

        public override bool Process(string message)
        {
            var taskMessage = JsonConvert.DeserializeObject<SchoolUserInfoRequest>(message);
            if (taskMessage == null)
            {
                // 返回false 的时候回直接驳回此消息,表示处理不了
                return false;
            }
            try
            {
                using (var scope = _services.CreateScope())
                {
                    bool result = false;
                    try
                    {
                        var repository = scope.ServiceProvider.GetRequiredService<ISchoolUserRepository>();
                        result = repository.AddSchoolUser(taskMessage).Result;
                        if (result == true)
                        {
                            _logger.LogInformation($"消息认证同步成功：{message}");
                        }
                        else
                        {
                            _logger.LogWarning($"认证信息同步出错：{message}");
                        }
                    }
                    catch (Exception e)
                    {
                        result = false;
                        _logger.LogError(e,$"认证信息同步出错\ntrace:{e.Message}");
                    }

                    return result;
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Process fail,error:{ex.Message},stackTrace:{ex.StackTrace},message:{message}");
                _logger.LogError(-1, ex, "Process fail");
                return false;
            }

        }

    }
}
