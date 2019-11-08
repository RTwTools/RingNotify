using Microsoft.Extensions.Configuration;
using System;

namespace RingNotify.Tests
{
  public abstract class TestBase
  {
    public IConfiguration AppSettings 
    { 
      get
      {
        return new ConfigurationBuilder()
          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .Build();
      }
    }
  }
}
