﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Windows.Media;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Editor.Shared.Extensions;
using Microsoft.CodeAnalysis.Shared.Extensions;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.Language.Intellisense;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.Editor.Implementation.IntelliSense.Completion.Presentation
{
    internal sealed class CustomCommitCompletion : Microsoft.VisualStudio.Language.Intellisense.Completion3, ICustomCommit
    {
        private static readonly string s_glyphCompletionWarning = "GlyphCompletionWarning";
        private readonly CompletionPresenterSession _completionPresenterSession;
        internal readonly CompletionItem CompletionItem;
        private readonly string _displayText;
        private readonly ImageMoniker _imageMoniker;

        public CustomCommitCompletion(CompletionPresenterSession completionPresenterSession, CompletionItem completionItem, string displayText)
            : base()
        {
            // PERF: Note that the base class contains a constructor taking the displayText string
            // but we're intentionally NOT using that here because it allocates a private CompletionState
            // object. By overriding the public property getters (DisplayText, InsersionText, etc.) the
            // extra allocation is avoided.
            _completionPresenterSession = completionPresenterSession;
            this.CompletionItem = completionItem;
            _displayText = displayText;
            _imageMoniker = CompletionItem.Glyph.HasValue ? CompletionItem.Glyph.Value.GetImageMoniker() : default(ImageMoniker);
        }

        public void Commit()
        {
            // If a commit happens through the UI then let the session know.  It will, in turn,
            // let the underlying controller know, and the controller can commit the completion
            // item.
            _completionPresenterSession.OnCompletionItemCommitted(CompletionItem);
        }

        public override string DisplayText
        {
            get
            {
                return _displayText;
            }
        }

        public override string InsertionText
        {
            get
            {
                return _displayText; // [sic] Same as DisplayText
            }
        }

        public override string Description
        {
            get
            {
                return this.CompletionItem.GetDescriptionAsync(CancellationToken.None).WaitAndGetResult(CancellationToken.None).GetFullText();
            }
        }

        public override ImageMoniker IconMoniker
        {
            get
            {
                return _imageMoniker;
            }
        }

        public override string IconAutomationText
        {
            get
            {
                return _imageMoniker.ToString();
            }
        }

        public override System.Collections.Generic.IEnumerable<CompletionIcon> AttributeIcons
        {
            get
            {
                if (this.CompletionItem.ShowsWarningIcon)
                {
                    var glyph = _completionPresenterSession.GlyphService.GetGlyph(StandardGlyphGroup.GlyphCompletionWarning, StandardGlyphItem.GlyphItemPublic);
                    return new[] { new CompletionIcon2(Glyph.CompletionWarning.GetImageMoniker(), s_glyphCompletionWarning, s_glyphCompletionWarning) };
                }

                return null;
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
