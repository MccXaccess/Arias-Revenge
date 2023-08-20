using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialogs.Data
{
    public class SpeechNodeData: DialogNodeBase
    {
        [SerializeField]
        private string _speakerName;

        [SerializeField]
        private string _text;

        [SerializeField]
        private Sprite _speakerSprite;

        [SerializeField]
        private AudioClip _audioClip;

        [SerializeField]
        private PositionType _positionType;

        [HideInInspector]
        [SerializeField]
        private List<SpeechNodeData> _connections;

        public string SpeakerName => _speakerName;
        public string Text => _text;
        public Sprite SpeakerSprite => _speakerSprite;
        public AudioClip AudioClip => _audioClip;
        public PositionType NarratorPosition => _positionType;
        public List<SpeechNodeData> Connections => _connections;

#if UNITY_EDITOR

        public void SetSpeakerName(string speakerName)
        {
            _speakerName = speakerName;
            AssetDatabase.SaveAssets();
        }

        public void SetText(string text)
        {
            _text = text;
            AssetDatabase.SaveAssets();
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            _audioClip = audioClip;
            AssetDatabase.SaveAssets();
        }

        public void SetPositionType(PositionType positionType)
        {
            _positionType = positionType;
            AssetDatabase.SaveAssets();
        }

        public void AddConnection(SpeechNodeData speechNode)
        {
            if(_connections is null)
                _connections = new List<SpeechNodeData>();

            _connections.Add(speechNode);
            AssetDatabase.SaveAssets();
        }

        public void RemoveConnection(SpeechNodeData speechNode)
        {
            if (_connections is null)
                return;
            _connections.Remove(speechNode);
            AssetDatabase.SaveAssets();
        }
#endif

        public enum PositionType
        {
            Left,
            Right
        }
    }
}