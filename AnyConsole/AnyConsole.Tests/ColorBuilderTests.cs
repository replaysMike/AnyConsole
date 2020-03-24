using System;
using NUnit.Framework;

namespace AnyConsole.Tests
{
    public class ColorBuilderTests
    {
        [Test]
        public void ColorTextBuilder_String_ShouldAppend()
        {
            var builder = new ColorTextBuilder();
            builder.Append("Test");
            builder.Append("Two");
            builder.Append("Three");

            var str = builder.ToString();
            Assert.AreEqual("TestTwoThree", str);
        }

        [Test]
        public void ColorTextBuilder_String_ShouldAppendLine()
        {
            var builder = new ColorTextBuilder();
            builder.Append("Test");
            builder.AppendLine("Two");
            builder.Append("Three");

            var str = builder.ToString();
            Assert.AreEqual($"TestTwo{Environment.NewLine}Three", str);
        }

        [Test]
        public void ColorTextBuilder_ColorTextBuilder_ShouldAppend()
        {
            var builder = new ColorTextBuilder();
            builder.Append(ColorTextBuilder.Create.Append("Test"));
            builder.Append(ColorTextBuilder.Create.Append("Two"));
            builder.Append(ColorTextBuilder.Create.Append("Three"));

            var str = builder.ToString();
            Assert.AreEqual("TestTwoThree", str);
        }

        [Test]
        public void ColorTextBuilder_ColorTextBuilder_ShouldAppendLine()
        {
            var builder = new ColorTextBuilder();
            builder.Append(ColorTextBuilder.Create.Append("Test"));
            builder.AppendLine(ColorTextBuilder.Create.Append("Two"));
            builder.Append(ColorTextBuilder.Create.Append("Three"));

            var str = builder.ToString();
            Assert.AreEqual($"TestTwo{Environment.NewLine}Three", str);
        }

        [Test]
        public void ColorTextBuilder_ColorTextBuilder_ShouldInterlace()
        {
            var builder = new ColorTextBuilder();
            var builder2 = new ColorTextBuilder();
            builder.AppendLine("Left side:1");
            builder.AppendLine("Left side:2");
            builder2.AppendLine("Right side:1");
            builder2.AppendLine("Right side:2");

            var interlaced = builder.Interlace(builder2);
            var str = interlaced.ToString();
            Assert.AreEqual($"Left side:1Right side:1{Environment.NewLine}Left side:2Right side:2{Environment.NewLine}", str);
        }

        [Test]
        public void ColorTextBuilder_ColorTextBuilder_ShouldInterlaceWithSpacing()
        {
            var builder = new ColorTextBuilder();
            var builder2 = new ColorTextBuilder();
            builder.AppendLine("Left side:1");
            builder.AppendLine("Left side:2");
            builder2.AppendLine("Right side:1");
            builder2.AppendLine("Right side:2");

            var spacing = 10;
            var interlaced = builder.Interlace(builder2, spacing);
            var str = interlaced.ToString();
            Assert.AreEqual($"Left side:1{new string(' ', spacing)}Right side:1{Environment.NewLine}Left side:2{new string(' ', spacing)}Right side:2{Environment.NewLine}", str);
        }
    }
}
