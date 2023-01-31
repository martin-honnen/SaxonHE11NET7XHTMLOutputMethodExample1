using net.sf.saxon.s9api;
using net.liberty_development.SaxonHE11s9apiExtensions;
using System.Reflection;

// force loading of updated xmlresolver
ikvm.runtime.Startup.addBootClassPathAssembly(Assembly.Load("org.xmlresolver.xmlresolver"));
ikvm.runtime.Startup.addBootClassPathAssembly(Assembly.Load("org.xmlresolver.xmlresolver_data"));

var processor = new Processor(false);

var xml = @"<Contact><Name>hello &lt;script&gt;alert('!')&lt;/script&gt;</Name></Contact>";

var xslt = @"<xsl:stylesheet version=""3.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
<xsl:output method=""xhtml"" indent=""yes"" html-version=""5.0"" doctype-system=""about:legacy-compat"" omit-xml-declaration=""yes""/>
<xsl:template match=""/"">
    <span data-title=""{{ 'title': '{/Contact/Name}' }}"">
        Name: <xsl:value-of select=""/Contact/Name""/>
        Input: <input type=""text"" value=""{/Contact/Name}""/>
    </span>
</xsl:template></xsl:stylesheet>
";

var xslt30Transformer = processor.newXsltCompiler().compile(xslt.AsSource()).load30();

var inputDoc = processor.newDocumentBuilder().build(xml.AsSource());

using var resultWriter = new StringWriter();

xslt30Transformer.applyTemplates(inputDoc, processor.NewSerializer(resultWriter));

var result = resultWriter.ToString();

Console.WriteLine(result);