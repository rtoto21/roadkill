﻿using System;
using System.IO;
using System.Text;
using Markdig;
using Markdig.Renderers;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Text.Parsers.Markdig
{
	public class MarkdigParser : IMarkupParser
	{
		public Func<HtmlImageTag, HtmlImageTag> ImageParsed { get; set; }
		public Func<HtmlLinkTag, HtmlLinkTag> LinkParsed { get; set; }

		public string ToHtml(string markdown)
		{
			if (string.IsNullOrEmpty(markdown))
				return "";

			var pipeline = new MarkdownPipelineBuilder()
                                .UseAdvancedExtensions()
                                .Build();

			var doc = Markdown.Parse(markdown, pipeline);
			var walker = new MarkdigAstWalker((e) => ImageParsed(e),
											  (e) => LinkParsed(e));

			walker.WalkAndBindParseEvents(doc);

			var builder = new StringBuilder();
			var textwriter = new StringWriter(builder);

			var renderer = new HtmlRenderer(textwriter);
			renderer.Render(doc);

			return builder.ToString();
		}
	}
}