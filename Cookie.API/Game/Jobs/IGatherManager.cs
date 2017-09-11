using System.Collections.Generic;
using Cookie.API.Game.Map.Elements;
using System;

namespace Cookie.API.Game.Jobs
{
    public interface IGatherManager
    {
        int Id { get; }
        bool IsFishing { get; }
        bool Moved { get; }
        bool Launched { get; }
        List<int> ToGather { get; }
        bool AutoGather { get; }

        void Launch();

        void Stop();

        void DoAutoGather(bool arg);

        void Gather();

        void Gather(List<int> resourcesId, bool autoGather, bool resetCounters = false);

        bool CanGatherOnMap(List<int> ids);

        List<IUsableElement> TrierDistanceElement(List<int> listDistance,
            List<IUsableElement> listUsableElement);

        event EventHandler GatherFinished;
    }
}