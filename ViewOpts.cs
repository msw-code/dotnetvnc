//  Copyright (C) 2004-2005 Rocky Lo. All Rights Reserved.
//
//  This file is part of the VNC system.
//
//  The VNC system is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307,
//  USA.
//
// If the source code for the VNC system is not available from the place 
// whence you received this file, check http://www.uk.research.att.com/vnc or contact
// the authors on vnc@uk.research.att.com for information on obtaining it.

using System;
using System.Xml;

namespace Vnc.Viewer
{
  internal enum PixelSize
  {
    Force8Bit,
    Force16Bit,
    Unspec
  }

  internal enum Orientation
  {
    Portrait,
    Landscape90,
    Portrait180,
    Landscape270
  }

  internal enum CliScaling
  {
    None,
    OneHalf,
    OneThird,
    OneFourth,
    OneFifth,
    Double,
    Auto,
    Custom
  }

  /// <remarks>This class is responsible for loading and saving view options.</remarks>
  internal class ViewOpts
  {
    private const string SettingsName = "Settings";
    private const string PixelSizeName = "PixelSize";
    private const string Force8BitName = "Force 8-bit";
    private const string Force16BitName = "Force 16-bit";
    private const string UnspecName = "Server decides";
    private const string IsFullScrnName = "FullScreen";
    private const string OrientationName = "Orientation";
    private const string PortraitName = "Portrait";
    private const string Landscape90Name = "Screen rotated clockwise";
    private const string Portrait180Name = "Upside down";
    private const string Landscape270Name = "Screen rotated counter-clockwise";
    private const string CliScalingName = "ClientScaling";
    private const string NoneName = "None";
    private const string OneHalfName = "OneHalf";
    private const string OneThirdName = "OneThird";
    private const string OneFourthName = "OneFourth";
    private const string OneFifthName = "OneFifth";
    private const string DoubleName = "Double";
    private const string AutoName = "Auto";
    private const string CustomName = "Custom";
    private const string WidthName = "Width";
    private const string HeightName = "Height";
    private const string ShareServName = "ShareServer";
    private const string ViewOnlyName = "ViewOnly";
    private const string SendMouseLocWhenIdleName = "SendMouseLocationWhenIdle";

    internal PixelSize PixelSize = PixelSize.Force8Bit;
    internal bool IsFullScrn = false;
    internal Orientation Orientation = Orientation.Portrait;
    internal CliScaling CliScaling = CliScaling.None;
    internal UInt16 CliScalingWidth = 0;
    internal UInt16 CliScalingHeight = 0;
    internal bool ShareServ = true;
    internal bool ViewOnly = false;
    internal bool SendMouseLocWhenIdle = false;

    internal ViewOpts()
    {
      // Do nothing. Using hardcoded defaults.
    }

    internal ViewOpts(string fileName)
    {
      Load(fileName);
    }

    // .NET CF does not support serialization.
    // We have to implement our own scheme.

    internal void Save(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      XmlElement elem = doc.CreateElement(SettingsName);
      doc.AppendChild(elem);

      AddToXml(doc, elem);

      doc.Save(fileName);
    }

    internal void AddToXml(XmlDocument doc, XmlElement rootElem)
    {
      XmlElement elem = doc.CreateElement(PixelSizeName);
      switch(PixelSize)
      {
        case PixelSize.Force16Bit:
          elem.InnerText = Force16BitName;
          break;
        case PixelSize.Unspec:
          elem.InnerText = UnspecName;
          break;
        default:
          elem.InnerText = Force8BitName;
          break;
      }
      rootElem.AppendChild(elem);

      elem = doc.CreateElement(IsFullScrnName);
      elem.InnerText = IsFullScrn.ToString();
      rootElem.AppendChild(elem);

      elem = doc.CreateElement(OrientationName);
      switch(Orientation)
      {
        case Orientation.Landscape90:
          elem.InnerText = Landscape90Name;
          break;
        case Orientation.Landscape270:
          elem.InnerText = Landscape270Name;
          break;
        case Orientation.Portrait180:
          elem.InnerText = Portrait180Name;
          break;
        default:
          elem.InnerText = PortraitName;
          break;
      }
      rootElem.AppendChild(elem);

      elem = doc.CreateElement(CliScalingName);
      switch(CliScaling)
      {
        case CliScaling.OneHalf:
          elem.InnerText = OneHalfName;
          break;
        case CliScaling.OneThird:
          elem.InnerText = OneThirdName;
          break;
        case CliScaling.OneFourth:
          elem.InnerText = OneFourthName;
          break;
        case CliScaling.OneFifth:
          elem.InnerText = OneFifthName;
          break;
        case CliScaling.Double:
          elem.InnerText = DoubleName;
          break;
        case CliScaling.Auto:
          elem.InnerText = AutoName;
          break;
        case CliScaling.Custom:
          elem.InnerText = CustomName;
          break;
        default:
          elem.InnerText = NoneName;
          break;
      }
      elem.SetAttribute(WidthName, CliScalingWidth.ToString());
      elem.SetAttribute(HeightName, CliScalingHeight.ToString());
      rootElem.AppendChild(elem);

      elem = doc.CreateElement(ShareServName);
      elem.InnerText = ShareServ.ToString();
      rootElem.AppendChild(elem);

      elem = doc.CreateElement(ViewOnlyName);
      elem.InnerText = ViewOnly.ToString();
      rootElem.AppendChild(elem);

      elem = doc.CreateElement(SendMouseLocWhenIdleName);
      elem.InnerText = SendMouseLocWhenIdle.ToString();
      rootElem.AppendChild(elem);
    }

    internal void Load(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);

      ReadFromXml(doc, doc.DocumentElement);
    }

    private XmlElement FindXmlElem(XmlDocument doc, XmlElement rootElem, string tagName)
    {
      XmlNodeList list = doc.GetElementsByTagName(tagName);
      for(int i = 0; i < list.Count; i++)
        if(list[i].ParentNode == rootElem)
          return (XmlElement)list[i];
      throw new FormatException("The specified element [" + tagName + "] is not found.");
    }

    internal void ReadFromXml(XmlDocument doc, XmlElement rootElem)
    {
      bool elemExists = false;

      XmlElement elem = FindXmlElem(doc, rootElem, PixelSizeName);
      switch(elem.InnerText)
      {
        case Force8BitName:
          PixelSize = PixelSize.Force8Bit;
          break;
        case Force16BitName:
          PixelSize = PixelSize.Force16Bit;
          break;
        case UnspecName:
          PixelSize = PixelSize.Unspec;
          break;
        default:
          throw new FormatException("PixelSize is unknown.");
      }

      elem = FindXmlElem(doc, rootElem, IsFullScrnName);
      IsFullScrn = Boolean.Parse(elem.InnerText);

      elem = FindXmlElem(doc, rootElem, OrientationName);
      switch(elem.InnerText)
      {
        case PortraitName:
          Orientation = Orientation.Portrait;
          break;
        case Portrait180Name:
          Orientation = Orientation.Portrait180;
          break;
        case Landscape90Name:
          Orientation = Orientation.Landscape90;
          break;
        case Landscape270Name:
          Orientation = Orientation.Landscape270;
          break;
        default:
          throw new FormatException("Orientation is unknown.");
      }

      try
      {
        elem = FindXmlElem(doc, rootElem, CliScalingName);
        elemExists = true;
      }
      catch(FormatException)
      {
        elemExists = false;
      }
      if(elemExists)
      {
        switch(elem.InnerText)
        {
          case NoneName:
            CliScaling = CliScaling.None;
            break;
          case OneHalfName:
            CliScaling = CliScaling.OneHalf;
            break;
          case OneThirdName:
            CliScaling = CliScaling.OneThird;
            break;
          case OneFourthName:
            CliScaling = CliScaling.OneFourth;
            break;
          case OneFifthName:
            CliScaling = CliScaling.OneFifth;
            break;
          case DoubleName:
            CliScaling = CliScaling.Double;
            break;
          case AutoName:
            CliScaling = CliScaling.Auto;
            break;
          case CustomName:
            CliScaling = CliScaling.Custom;
            break;
          default:
            throw new FormatException("Client-side scaling setting is unknown.");
        }
        try
        {
          CliScalingWidth = UInt16.Parse(elem.GetAttribute(WidthName));
          CliScalingHeight = UInt16.Parse(elem.GetAttribute(HeightName));
        }
        catch(ArgumentNullException)
        {
          throw new FormatException("Client-side scaling setting is invalid.");
        }
        catch(OverflowException)
        {
          throw new FormatException("Client-side scaling setting is invalid.");
        }
        catch(FormatException)
        {
          throw new FormatException("Client-side scaling setting is invalid.");
        }
      }
      else
      {
        CliScaling = CliScaling.None;
        CliScalingWidth = 0;
        CliScalingHeight = 0;
      }

      elem = FindXmlElem(doc, rootElem, ViewOnlyName);
      ViewOnly = Boolean.Parse(elem.InnerText);

      elem = FindXmlElem(doc, rootElem, ShareServName);
      ShareServ = Boolean.Parse(elem.InnerText);

      try
      {
        elem = FindXmlElem(doc, rootElem, SendMouseLocWhenIdleName);
        SendMouseLocWhenIdle = Boolean.Parse(elem.InnerText);
      }
      catch(FormatException)
      {
        SendMouseLocWhenIdle = false;
      }
    }
  }
}
