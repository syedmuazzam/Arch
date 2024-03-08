using System;
using System.Collections.Generic;

namespace ArchEditor.Stores;

internal interface IUndoRedoAction
{
    string Name { get; }
    void Undo();
    void Redo();
}

internal class UndoRedoAction : IUndoRedoAction
{
    private readonly Action? _undoAction;
    private readonly Action? _redoAction;

    public string Name { get; }
    public void Undo() => _undoAction?.Invoke();
    public void Redo() => _redoAction?.Invoke();

    public UndoRedoAction(string name)
    {
        Name = name;
    }

    public UndoRedoAction(string name, Action undoAction, Action redoAction) : this(name)
    {
        _undoAction = undoAction;
        _redoAction = redoAction;
    }
}

/// <summary>
/// Stores undo and redo actions
/// </summary>
internal class UndoRedoStore
{
    private readonly Stack<IUndoRedoAction> _undoStack = new();
    private readonly Stack<IUndoRedoAction> _redoStack = new();

    /// <summary>
    /// Adds an action to the undo stack
    /// </summary>
    /// <param name="action"></param>
    public void AddUndoAction(IUndoRedoAction action)
    {
        _undoStack.Push(action);
        _redoStack.Clear();
    }

    /// <summary>
    /// Undoes the last action
    /// </summary>
    public void Undo()
    {
        if (_undoStack.Count == 0) return;
        IUndoRedoAction action = _undoStack.Pop();
        action.Undo();
        _redoStack.Push(action);
    }

    /// <summary>
    /// Redoes the last action
    /// </summary>
    public void Redo()
    {
        if (_redoStack.Count == 0) return;
        IUndoRedoAction action = _redoStack.Pop();
        action.Redo();
        _undoStack.Push(action);
    }
}

