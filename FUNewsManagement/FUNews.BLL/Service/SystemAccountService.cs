using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;

namespace FUNews.BLL.Service
{
    public class SystemAccountService(ISystemAccountRepository SystemAccountRepository, IMapper mapper)
        : BaseService<SystemAccount, short>(SystemAccountRepository), ISystemAccountService
    {
    }
}
