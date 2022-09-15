﻿using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Layers
{
    public class LinkLayer : BaseLayer<BaseLinkModel>
    {
        public LinkLayer(Diagram diagram) : base(diagram) { }

        protected override void OnItemAdded(BaseLinkModel link)
        {
            link.Diagram = Diagram;
            HandleAnchor(link, link.Source, true);
            if (link.Target != null) HandleAnchor(link, link.Target, true);
            link.Refresh();

            link.SourceChanged += OnLinkSourceChanged;
            link.TargetChanged += OnLinkTargetChanged;
        }

        protected override void OnItemRemoved(BaseLinkModel link)
        {
            link.Diagram = null;
            HandleAnchor(link, link.Source, false);
            if (link.Target != null) HandleAnchor(link, link.Target, false);
            link.Refresh();

            link.SourceChanged -= OnLinkSourceChanged;
            link.TargetChanged -= OnLinkTargetChanged;
            
            Diagram.Controls.RemoveFor(link);
        }

        private static void OnLinkSourceChanged(BaseLinkModel link, Anchor old, Anchor @new)
        {
            HandleAnchor(link, old, add: false);
            HandleAnchor(link, @new, add: true);
        }

        private static void OnLinkTargetChanged(BaseLinkModel link, Anchor? old, Anchor? @new)
        {
            if (old != null) HandleAnchor(link, old, add: false);
            if (@new != null) HandleAnchor(link, @new, add: true);
        }

        private static void HandleAnchor(BaseLinkModel link, Anchor anchor, bool add)
        {
            if (add)
            {
                anchor.Model?.AddLink(link);
            }
            else
            {
                anchor.Model?.RemoveLink(link);
            }

            if (anchor.Model is Model model)
            {
                model.Refresh();
            }
        }
    }
}
