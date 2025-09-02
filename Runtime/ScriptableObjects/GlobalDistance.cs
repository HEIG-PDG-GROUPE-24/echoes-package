using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

namespace Echoes.Runtime.ScriptableObjects
{
    [Serializable]
    public class GlobalDistance
    {
        [Required] [OnValueChanged(nameof(ValidateLimits))]
        public double minValue = 0;

        [Required] [OnValueChanged(nameof(ValidateLimits))]
        public double maxValue = double.MaxValue;

        private void ValidateLimits()
        {
            if (minValue > maxValue)
                (minValue, maxValue) = (maxValue, minValue);
        }

        [Button("Update", ButtonSizes.Large)]
        public void Update()
        {
            HashSet<string> visitedPairs = new();
            // iterates over all npcs and their contacts to add the pair if it does not exist yet
            foreach (var npcComponent in EchoesGlobal.GetAllNPCs())
            {
                Debug.LogFormat("Checking {0}'s contacts", npcComponent.npcData.Name);
                foreach (var contact in npcComponent.npcData.Contacts.Where(contact => contact != null))
                {
                    visitedPairs.Add(contact.npcData.Name + npcComponent.npcData.Name);
                    if (DistanceExists(npcComponent.npcData.Name, contact.npcData.Name)) continue;

                    Debug.LogFormat("Contact pair {0} - {1} did not have a distance.", contact.npcData.Name,
                        npcComponent.npcData.Name);
                    if (!_distancesBetweenContacts.ContainsKey(npcComponent.npcData.Name))
                        _distancesBetweenContacts.Add(npcComponent.npcData.Name, new Dictionary<string, double>());
                    _distancesBetweenContacts[npcComponent.npcData.Name]
                        .Add(contact.npcData.Name, (maxValue + minValue) / 2);
                }
            }

            // any distance entry that wasn't visited should no longer exist
            foreach (var key1 in _distancesBetweenContacts.Keys.ToList())
            {
                foreach (var key2 in _distancesBetweenContacts[key1].Keys.ToList().Where(key2 =>
                             !visitedPairs.Contains(key1 + key2) && !visitedPairs.Contains(key2 + key1)))
                {
                    Debug.LogFormat("Removing pair {0} - {1}", key1, key2);
                    _distancesBetweenContacts[key1].Remove(key2);
                    if (_distancesBetweenContacts[key1].Count == 0)
                        _distancesBetweenContacts.Remove(key1);
                }
            }

            UpdateNpcs();
        }


        [HideInInspector][OdinSerialize]
        private Dictionary<string, Dictionary<string, double>>
            _distancesBetweenContacts = new();

        private bool DistanceExists(string npc1, string npc2)
        {
            if (_distancesBetweenContacts.ContainsKey(npc1) && _distancesBetweenContacts[npc1].ContainsKey(npc2))
                return true;
            return (_distancesBetweenContacts.ContainsKey(npc2) && _distancesBetweenContacts[npc2].ContainsKey(npc1));
        }

        private void UpdateNpcs()
        {
            foreach (var npc in EchoesGlobal.GetAllNPCs())
                npc.npcData.SyncWithGlobalDistances();
        }

        public void SetContactDistance(string npc1, string npc2, double distance)
        {
            if (distance > maxValue || distance < minValue)
                throw new ArgumentOutOfRangeException(nameof(distance), distance,
                    "Distance should be between min and max inclusive");
            if (_distancesBetweenContacts.ContainsKey(npc1) && _distancesBetweenContacts[npc1].ContainsKey(npc2))
                _distancesBetweenContacts[npc1][npc2] = distance;
            else if (_distancesBetweenContacts.ContainsKey(npc2) && _distancesBetweenContacts[npc2].ContainsKey(npc1))
                _distancesBetweenContacts[npc2][npc1] = distance;
            else
            {
                if (!_distancesBetweenContacts.ContainsKey(npc1))
                    _distancesBetweenContacts.Add(npc1, new Dictionary<string, double>());
                _distancesBetweenContacts[npc1].Add(npc2, distance);
            }

            UpdateNpcs();
        }

        public double GetContactDistance(string npc1, string npc2)
        {
            if (_distancesBetweenContacts.ContainsKey(npc1) && _distancesBetweenContacts[npc1].ContainsKey(npc2))
                return _distancesBetweenContacts[npc1][npc2];
            if (_distancesBetweenContacts.ContainsKey(npc2) && _distancesBetweenContacts[npc2].ContainsKey(npc1))
                return _distancesBetweenContacts[npc2][npc1];
            throw new ArgumentException("Unknown contact pair");
        }

        public List<KeyValuePair<string, double>> ListContactsWithDistanceOf(string npc)
        {
            var contacts = (from npcKey in _distancesBetweenContacts.Keys
                    where _distancesBetweenContacts[npcKey].ContainsKey(npc)
                    select new KeyValuePair<string, double>(npcKey, _distancesBetweenContacts[npcKey][npc]))
                .ToList();

            if (!_distancesBetweenContacts.ContainsKey(npc)) return contacts;

            var otherContacts = _distancesBetweenContacts[npc];
            contacts.AddRange(otherContacts.Keys.Select(otherContactKey =>
                new KeyValuePair<string, double>(otherContactKey, otherContacts[otherContactKey])));

            return contacts;
        }
    }
}