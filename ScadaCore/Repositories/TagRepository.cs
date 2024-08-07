﻿using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class TagRepository : ITagRepository {
    private const string XmlFilePath = "Config/scadaConfig.xml";

    public TagRepository() {
        Directory.CreateDirectory("Config");
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

    public async Task<Tag?> GetTagByInputOutputAddressAsync(int inputOutputAddress) {
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
            if (tagXElement.Element(Tag.GetInputOutputAddressXElementName())?.Value != inputOutputAddress.ToString())
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

    public async Task<ICollection<Tag>> GetAllInputTags() {
        var rootElement = await GetRootElement();
        
        var tagTypes =
            new [] {
                AnalogInputTag.GetXName,
                DigitalInputTag.GetXName
            };
        var tagsXElements =
            rootElement.Descendants().Where(descendant => tagTypes.Contains(descendant.Name.LocalName));
        
        var tags = new Collection<Tag>();
        foreach (var tagXElement in tagsXElements)
        {
            switch (tagXElement.Name.LocalName) {
                case AnalogInputTag.GetXName:
                    tags.Add(new AnalogInputTag(tagXElement));
                    break;
                case DigitalInputTag.GetXName:
                    tags.Add(new DigitalInputTag(tagXElement));
                    break;
            }
        }
        return tags;
    }

    private static async Task SaveXElementAsync(XElement xElement) {
        await using var xmlWriter = XmlWriter.Create(XmlFilePath, new XmlWriterSettings{Async = true});
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
    public async Task<Collection<Tag>> GetAllOutputTags()
    {
        var rootElement = await GetRootElement();
        var tagTypes =
            new [] {
                AnalogOutputTag.GetXName,
                DigitalOutputTag.GetXName
            };
        var tagsXElements =
            rootElement.Descendants().Where(descendant => tagTypes.Contains(descendant.Name.LocalName));
        
        Collection<Tag> res = new Collection<Tag>();
        foreach (var tagXElement in tagsXElements)
        {
            switch (tagXElement.Name.LocalName) {
                case AnalogOutputTag.GetXName:
                    res.Add(new AnalogOutputTag(tagXElement));
                    break;
                case DigitalOutputTag.GetXName:
                     res.Add(new DigitalOutputTag(tagXElement));
                    break;
            }
        }
        return res;
    }
}