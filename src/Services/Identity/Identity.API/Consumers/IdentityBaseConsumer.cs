﻿using Identity.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Identity.API.Consumers
{
    public class IdentityBaseConsumer<T>
    {
        protected readonly IUserService UserService;
        protected readonly ILogger<T> Logger;
        public IdentityBaseConsumer(IUserService userService, ILogger<T> logger)
        {
            UserService = userService;
            Logger = logger;
        }
    }
}
