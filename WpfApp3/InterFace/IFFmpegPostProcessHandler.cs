using HaruaConvert.Parameter;
using System.Threading.Tasks;

namespace HaruaConvert.HaruaInterFace
{
    public interface IFFmpegPostProcessHandler
    {
        Task HandleAfterProcessExitAsync(ParamField paramField);
    }
}
