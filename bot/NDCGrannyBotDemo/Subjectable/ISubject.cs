using System.Threading.Tasks;
using NDCGrannyBotDemo.Context;

namespace NDCGrannyBotDemo.Subjectable
{
    public interface ISubject
    {

        string SubjectName { get; set; }

        Task<bool> StartSubject(V4ReferenceContext context);


        Task<bool> ContinueSubject(V4ReferenceContext context);

        ISubject ParentSubject { get; set; }

    }
}
