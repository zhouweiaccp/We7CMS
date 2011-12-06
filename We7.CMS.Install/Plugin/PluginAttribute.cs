using System;

namespace We7.CMS
{
  /// <summary>
  /// All extensions must decorate the class with this attribute.
  /// It is used for reflection.
  /// <remarks>
  /// When using this attribute, you must make sure
  /// to have a default constructor. It will be used to create
  /// an instance of the extension through reflection.
  /// </remarks>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public  class PluginAttribute : Attribute
  {
    /// <summary>
    /// Creates an instance of the attribute and assigns a description.
    /// </summary>
    public PluginAttribute(string description, string version, string author)
    {
      _Description = description;
      _Version = version;
      _Author = author;
    }

    /// <summary>
    /// Creates an instance of the attribute and assigns a description.
    /// </summary>
    public PluginAttribute(string description, string version, string author, int priority)
    {
        _Description = description;
        _Version = version;
        _Author = author;
        _priority = priority;
    }

    private string _Description;
    /// <summary>
    /// Gets the description of the extension.
    /// </summary>
    public string Description
    {
      get { return _Description; }
    }

    private string _Version;

    /// <summary>
    /// Gets the version number of the extension
    /// </summary>
    public string Version
    {
      get { return _Version; }
    }

    private string _Author;

    /// <summary>
    /// Gets the author of the extension
    /// </summary>
    public string Author
    {
      get { return _Author; }
    }

    private int _priority;

    /// <summary>
    /// Gets the priority of the extension
    /// This determins in what order extensions instantiated
    /// and in what order they will respond to events
    /// </summary>
    public int Priority
    {
        get { return _priority; }
    }

  }

  /// <summary>
  /// Helper class for sorting extensions by priority
  /// </summary>
  public class SortedPlugin
  {
      /// <summary>
      /// Order in which extensions are sorted and respond to events
      /// </summary>
      public int Priority;
      /// <summary>
      /// Name of the extension
      /// </summary>
      public string Name;
      /// <summary>
      /// Type of the extension
      /// </summary>
      public string Type;
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="p">Priority</param>
      /// <param name="n">Name</param>
      /// <param name="t">Type</param>
      public SortedPlugin(int p, string n, string t)
      {
          Priority = p;
          Name = n;
          Type = t;
      }
  }    
}