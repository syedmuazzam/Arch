using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ArchEditor.Utilities
{
	internal static class Serializer
	{
		/// <summary>
		/// Serializes an instance of generic type T to a file at the given path.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <param name="path"></param>
		internal static void ToFile<T>(T instance, string path)
		{
			try
			{
				using FileStream fs = new(path, FileMode.Create);
				DataContractSerializer serializer = new(typeof(T));
				serializer.WriteObject(fs, instance);
			}
			catch (Exception ex)
			{
				// TODO: log error
				Debug.WriteLine(ex.ToString());
			}
		}

		/// <summary>
		/// Serializes an instance of generic type T to XML as XDocument.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <returns></returns>
		internal static XDocument? ToXmlDoc<T>(T instance)
		{
			try
			{
				using MemoryStream ms = new();
				DataContractSerializer serializer = new(typeof(T));
				serializer.WriteObject(ms, instance);
				ms.Position = 0;
				using StreamReader sr = new(ms);
				return XDocument.Parse(sr.ReadToEnd());
			}
			catch (Exception ex)
			{
				// TODO: log error
				Debug.WriteLine(ex.ToString());
				return null;
			}
		}

		/// <summary>
		/// Serializes an instance of generic type T to XML as XElement.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="instance"></param>
		/// <returns></returns>
		internal static XElement? ToXmlElement<T>(T instance)
		{
			try
			{
				using MemoryStream ms = new();
				DataContractSerializer serializer = new(typeof(T));
				serializer.WriteObject(ms, instance);
				ms.Position = 0;
				using StreamReader sr = new(ms);
				return XElement.Parse(sr.ReadToEnd());
			}
			catch (Exception ex)
			{
				// TODO: log error
				Debug.WriteLine(ex.ToString());
				return null;
			}
		}

		/// <summary>
		/// Deserializes an instance of generic type T from a file at the given path.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path">
		/// The path to the file to deserialize from.
		/// </param>
		/// <returns>
		/// Returns an instance of generic type T if successful, otherwise returns null.
		/// </returns>
		internal static T? FromFile<T>(string path)
		{
			try
			{
				using var fs = new FileStream(path, FileMode.Open);
				DataContractSerializer serializer = new(typeof(T));
				T? instance = (T?)serializer.ReadObject(fs);
				return instance;
			}
			catch (Exception ex)
			{
				// TODO: log error
				Debug.WriteLine(ex.ToString());
				return default;
			}
		}
	}
}
