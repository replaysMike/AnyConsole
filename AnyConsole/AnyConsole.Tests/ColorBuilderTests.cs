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
        public void ColorTextBuilder_ColorTextBuilder_ShouldInterlaceInColumns()
        {
            var builder = new ColorTextBuilder();
            var builder2 = new ColorTextBuilder();
            builder.AppendLine("Left");
            builder.AppendLine("Left side:2");
            builder2.AppendLine("Right");
            builder2.AppendLine("Right side:2");

            var interlaced = builder.Interlace(builder2, fixedColumnSpacing: 2, fixedColumnWidth: 15);
            var str = interlaced.ToString();
            // data should be seperated into 2 columns 15 chars wide, plus 2 char padding
            Assert.AreEqual($"Left             Right{Environment.NewLine}Left side:2      Right side:2{Environment.NewLine}", str);
        }

        [Test]
        public void ColorTextBuilder_MoreLeftColorTextBuilder_ShouldInterlaceInColumns()
        {
            var builder = new ColorTextBuilder();
            var builder2 = new ColorTextBuilder();
            builder.AppendLine("Left");
            builder.AppendLine("Left side:2");
            builder.Append("Left");
            builder.AppendLine(" side:3");
            builder.AppendLine("Left side:4");
            builder2.AppendLine("Right");
            builder2.AppendLine("Right side:2");

            var interlaced = builder.Interlace(builder2, fixedColumnSpacing: 2, fixedColumnWidth: 15);
            var str = interlaced.ToString();
            // data should be seperated into 2 columns 15 chars wide, plus 2 char padding
            Assert.AreEqual($"Left             Right{Environment.NewLine}Left side:2      Right side:2{Environment.NewLine}Left side:3      {Environment.NewLine}Left side:4      {Environment.NewLine}", str);
        }

        [Test]
        public void ColorTextBuilder_MoreRightColorTextBuilder_ShouldInterlaceInColumns()
        {
            var builder = new ColorTextBuilder();
            var builder2 = new ColorTextBuilder();
            builder.AppendLine("Left");
            builder.AppendLine("Left side:2");
            builder2.AppendLine("Right");
            builder2.AppendLine("Right side:2");
            builder2.Append("Right");
            builder2.AppendLine(" side:3");
            builder2.AppendLine("Right side:4");

            var interlaced = builder.Interlace(builder2, fixedColumnSpacing: 2, fixedColumnWidth: 15);
            var str = interlaced.ToString();
            // data should be seperated into 2 columns 15 chars wide, plus 2 char padding
            Assert.AreEqual($"Left             Right{Environment.NewLine}Left side:2      Right side:2{Environment.NewLine}                 Right side:3{Environment.NewLine}                 Right side:4{Environment.NewLine}", str);
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

        [Test]
        public void ColorTextBuilder_GetWidth()
        {
            var builder = new ColorTextBuilder();
            builder.Append("Test");
            builder.Append("Two");
            builder.AppendLine("Three");
            builder.Append("Four");
            builder.Append("Five");
            builder.AppendLine("Six");
            builder.Append("Seven");
            builder.AppendLine("This is the final string");
            var width = builder.Width;
            Assert.AreEqual("SevenThis is the final string".Length, width);
        }

        [Test]
        public void ColorTextBuilder_GetHeight()
        {
            var builder = new ColorTextBuilder();
            builder.Append("Test");
            builder.Append("Two");
            builder.AppendLine("Three");
            builder.Append("Four");
            builder.Append("Five");
            builder.AppendLine("Six");
            builder.Append("Seven");
            builder.AppendLine("This is the final string");
            builder.Append("Fourth line");
            var height = builder.Height;
            Assert.AreEqual(4, height);
        }

        [Test]
        public void ColorTextBuilder_GetHeightTrailingNewline()
        {
            var builder = new ColorTextBuilder();
            builder.Append("Test");
            builder.Append("Two");
            builder.AppendLine("Three");
            builder.Append("Four");
            builder.Append("Five");
            builder.AppendLine("Six");
            builder.Append("Seven");
            builder.AppendLine("This is the final string");
            builder.AppendLine();
            var height = builder.Height;
            Assert.AreEqual(4, height);
        }
    }
}
