using AutoMapper;
using SchoolRegister.BLL.Entities;
using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.DTOs;
using SchoolRegister.ViewModels.VMs;
using System;
using System.Linq;

namespace SchoolRegister.Services.Services
{
    public class SubjectGroupService : BaseService, ISubjectGroupService
    {
        public SubjectGroupService(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public GroupVM AttachSubjectWithGroup(AttachDetachSubjectWithGroupDTO attachSubjectWithGroupDTO)
        {
            if (attachSubjectWithGroupDTO == null)
            {
                throw new ArgumentNullException($"DTO is null");
            }
            var subjectGroup = _dbContext.SubjectGroups.FirstOrDefault(sg => sg.GroupId == attachSubjectWithGroupDTO.GroupId && sg.SubjectId == attachSubjectWithGroupDTO.SubjectId);
            if (subjectGroup != null)
            {
                throw new ArgumentNullException($"Subject is already attached to this group");
            }
            subjectGroup = new SubjectGroup
            {
                GroupId = attachSubjectWithGroupDTO.GroupId,
                SubjectId = attachSubjectWithGroupDTO.SubjectId,
            };
            _dbContext.SubjectGroups.Add(subjectGroup);
            _dbContext.SaveChanges();
            var group = _dbContext.Groups.FirstOrDefault(x => x.Id == attachSubjectWithGroupDTO.GroupId);
            var groupVM = _mapper.Map<GroupVM>(group);
            return groupVM;
        }

        public GroupVM DetachSubjectFromGroup(AttachDetachSubjectWithGroupDTO detachSubjectWithGroupDTO)
        {
            if (detachSubjectWithGroupDTO == null)
            {
                throw new ArgumentNullException($"DTO is null");
            }
            var subjectGroup = _dbContext.SubjectGroups.FirstOrDefault(sg => sg.GroupId == detachSubjectWithGroupDTO.GroupId && sg.SubjectId == detachSubjectWithGroupDTO.SubjectId);
            if (subjectGroup == null)
            {
                throw new ArgumentNullException($"This subject is not attached with this group");
            }
            _dbContext.SubjectGroups.Remove(subjectGroup);
            _dbContext.Remove(subjectGroup);
            _dbContext.SaveChanges();
            var group = _dbContext.Groups.FirstOrDefault(x => x.Id == detachSubjectWithGroupDTO.GroupId);
            var groupVM = _mapper.Map<GroupVM>(group);
            return groupVM;
        }

    }
}
