using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class TagRepository : ITagRepository {
    private const string XmlFilePath = "~/Config/scadaConfig.xml";

    public TagRepository() {
        Directory.CreateDirectory("~/Config");
        if (File.Exists(XmlFilePath))
            return;
        
        var scadaConfigTemplate =
            new XElement(
                Tag.GetRootXElementName(),
                new XElement(AnalogInputTag.GetParentXElementName()),
                new XElement(AnalogOutputTag.GetParentXElementName()),
                new XElement(DigitalInputTag.GetParentXElementName()),
                new XElement(DigitalOutputTag.GetParentXElementName())
            );
        using var xmlWriter = XmlWriter.Create(XmlFilePath);
        scadaConfigTemplate.Save(xmlWriter);
    }
    
    private static async Task<XElement> GetRootElement() {
        using var xmlReader = XmlReader.Create(XmlFilePath, new XmlReaderSettings{ Async = true});
        return await XElement.LoadAsync(xmlReader, LoadOptions.None, CancellationToken.None);
    }
    
    public async Task<Tag?> GetTagAsync(string name) {
        var rootElement = await GetRootElement();

        var tagTypes =
            new [] {
                AnalogInputTag.GetXName,
                AnalogOutputTag.GetXName,
                DigitalInputTag.GetXName,
                DigitalOutputTag.GetXName
            };
        var tagsXElements =
            rootElement.Descendants().Where(descendant => tagTypes.Contains(descendant.Name.LocalName));

        foreach (var tagXElement in tagsXElements) {
            if (tagXElement.Element(Tag.GetNameXElementName())?.Value != name)
                continue;

            switch (tagXElement.Name.LocalName) {
                case AnalogInputTag.GetXName:
                    return new AnalogInputTag(tagXElement);
                case AnalogOutputTag.GetXName:
                    return new AnalogOutputTag(tagXElement);
                case DigitalInputTag.GetXName:
                    return new DigitalInputTag(tagXElement);
                case DigitalOutputTag.GetXName:
                    return new DigitalOutputTag(tagXElement);
            }
        }
        return null;
    }

    private static async Task SaveXElementAsync(XElement xElement) {
        await using var xmlWriter = XmlWriter.Create(XmlFilePath);
        await xElement.SaveAsync(xmlWriter, CancellationToken.None);
    }
    
    public async Task<Tag?> CreateTagAsync(Tag tag) {
        var rootElement = await GetRootElement();

        var tagType = ""; 
        XElement? tagXElement = null;
        switch (tag) {
            case AnalogInputTag analogInputTag:
                tagType = AnalogInputTag.GetParentXElementName();
                tagXElement = analogInputTag.GetXElementRepresentation();
                break;
            case AnalogOutputTag analogOutputTag:
                tagType = AnalogOutputTag.GetParentXElementName();
                tagXElement = analogOutputTag.GetXElementRepresentation();
                break;
            case DigitalInputTag digitalInputTag:
                tagType = DigitalInputTag.GetParentXElementName();
                tagXElement = digitalInputTag.GetXElementRepresentation();
                break;
            case DigitalOutputTag digitalOutputTag:
                tagType = DigitalOutputTag.GetParentXElementName();
                tagXElement = digitalOutputTag.GetXElementRepresentation();
                break;
        }
        var parentElement = rootElement.Element(tagType);
        if (parentElement == null || tagXElement == null)
            return null;
        
        parentElement.Add(tagXElement);

        await SaveXElementAsync(rootElement);
        return tag switch {
            AnalogInputTag => new AnalogInputTag(tagXElement),
            AnalogOutputTag => new AnalogOutputTag(tagXElement),
            DigitalInputTag => new DigitalInputTag(tagXElement),
            DigitalOutputTag => new DigitalOutputTag(tagXElement),
            _ => null
        };
    }
    
    public async Task<bool> DeleteTagAsync(Tag tag) {
        var rootElement = await GetRootElement();

        var tagToRemove =
            rootElement
                .Descendants()
                .FirstOrDefault(descendant =>
                    descendant.Name == Tag.GetNameXElementName() && descendant.Value == tag.Name)?
                .Parent;
        if (tagToRemove == null)
            return false;
        
        tagToRemove.Remove();

        await SaveXElementAsync(rootElement);
        return true;
    }
    public async Task<Collection<String>> GetAllInputTags()
    {
        var rootElement = await GetRootElement();
        var tagTypes =
            new [] {
                AnalogInputTag.GetXName,
                DigitalInputTag.GetXName
            };
        var tagsXElements =
            rootElement.Descendants().Where(descendant => tagTypes.Contains(descendant.Name.LocalName));
        Collection<String> res = new Collection<String>();
        foreach (var tagXElement in tagsXElements) res.Add(tagXElement.Element(Tag.GetNameXElementName())?.Value);
        return res;
    }
}