using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocabulary
{
    class Leitner
    {
        #region Variables
        private List<VocabularyLeitner> box0;
        private List<VocabularyLeitner> box1;
        private List<VocabularyLeitner> box2;
        private LinkedList<VocabularyLeitner> vocabulary;
        private VocabularyLeitner selectedVocabulary;
        private int session;
        private static Leitner instance = null;
        #endregion

        #region Constructor(s)
        private Leitner(){}
        #endregion

        #region Properties
        public static Leitner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Leitner();
                }
                return instance;
            }
        }

        public VocabularyLeitner SelectedVocabulary
        {
           get { return this.selectedVocabulary; }
        }
        #endregion

        #region Methods
        public void reset(List<VocabularyLeitner> alv)
        {
            this.session = 0;
            this.selectedVocabulary = null;
            this.vocabulary = new LinkedList<VocabularyLeitner>(alv);
            this.box0 = new List<VocabularyLeitner>(alv);
            this.box1 = new List<VocabularyLeitner>();
            this.box2 = new List<VocabularyLeitner>();
        }

        private void getVocabulary()
        {
            switch (this.session)
            {
                case 0:
                    this.addAllInLinkedList(this.box0);
                    break;
                case 1:
                    this.addAllInLinkedList(this.box0);
                    this.addAllInLinkedList(this.box1);
                    break;
                case 2:
                    this.addAllInLinkedList(this.box0);
                    this.addAllInLinkedList(this.box1);
                    this.addAllInLinkedList(this.box2);
                    break;
                default:
                    throw new Exception("[Leitner]Impossible d'obtenir plus de trois session");
            }
        }

        private void sort()
        {
            switch (this.session)
            {
                case 0:
                    this.sortVocabularySession0();
                    break;
                case 1:
                    this.sortVocabularySession1();
                    break;
                case 2:
                    this.sortVocabularySession2();
                    break;
            }
        }



        private void sortVocabularySession2()
        {
            foreach (VocabularyLeitner v in this.box2.ToList())
            {
                if (!v.Known)
                {
                    this.box0.Add(v);
                    this.box2.Remove(v);
                }
                else
                {
                    v.Known = false;
                }
            }
            this.sortVocabularySession1();
        }



        private void sortVocabularySession1()
        {
            foreach (VocabularyLeitner v in this.box1.ToList())
            {
                if (v.Known)
                {
                    this.box2.Add(v);
                    v.Known = false;
                }
                else
                {
                    this.box0.Add(v);
                }
                this.box1.Remove(v);
            }
            this.sortVocabularySession0();
        }



        private void sortVocabularySession0()
        {
            foreach (VocabularyLeitner v in this.box0.ToList())
            {
                if (v.Known)
                {
                    this.box1.Add(v);
                    v.Known = false;
                    this.box0.Remove(v);
                }
            }
        }

        public void next()
        {
            if (this.vocabulary.Count == 0)
            {
                this.sort();
                this.changeSession();
                ShuffleList.Shuffle<VocabularyLeitner>(this.box0);
                ShuffleList.Shuffle<VocabularyLeitner>(this.box1);
                ShuffleList.Shuffle<VocabularyLeitner>(this.box2);
                this.getVocabulary();
            }
            this.selectedVocabulary = this.vocabulary.First();
            this.vocabulary.RemoveFirst();
        }

        private void changeSession()
        {
            switch (this.session)
            {
                case 0:
                    if (this.box1.Count == 0)
                    {
                        if (this.box2.Count == 0)
                        {
                            this.session = 0;
                        }
                        else
                        {
                            this.session = 2;
                        }
                    }
                    else
                    {
                        this.session = 1;
                    }
                    break;
                case 1:
                    if (this.box2.Count == 0)
                    {
                        if (this.box0.Count == 0)
                        {
                            this.session = 1;
                        }
                        else
                        {
                            this.session = 0;
                        }
                    }
                    else
                    {
                        this.session = 2;
                    }
                    break;
                case 2:
                    if (this.box0.Count == 0)
                    {
                        if (this.box1.Count == 0)
                        {
                            this.session = 2;
                        }
                        else
                        {
                            this.session = 1;
                        }
                    }
                    else
                    {
                        this.session = 0;
                    }
                    break;
            }
        }

        public void addAllInLinkedList(List<VocabularyLeitner> list)
        {
            foreach (VocabularyLeitner v in list)
            {
                this.vocabulary.AddLast(new LinkedListNode<VocabularyLeitner>(v));
            }
        }

        public override string  ToString()
        {
            string res = null;
            res += "#######BOX0#########\n";
            foreach (VocabularyLeitner v in this.box0)
            {
                res += "\t" + v + "\n";
            }
            res += "#######BOX1#########\n";
            foreach (VocabularyLeitner v in this.box1)
            {
                res += "\t" + v + "\n";
            }
            res += "#######BOX2#########\n";
            foreach (VocabularyLeitner v in this.box2)
            {
                res += "\t" + v + "\n";
            }
            res += "SESSION : " + this.session + "\n";
            res += "SELCTEDCOLOR : " + this.SelectedVocabulary.ToString();

            return res;
        }
        #endregion
    }

}
