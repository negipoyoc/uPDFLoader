using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace BABINIKU.PDF
{
    /// <summary>
    /// Loadのサンプル
    /// </summary>
    public class PDFLoader : MonoBehaviour
    {
        /// <summary>
        /// ページ数0以上になるようにする。
        /// </summary>
        int _page;
        int page
        {
            get { return _page; }
            set
            {
                if (info == null)
                {
                    _page = Mathf.Max(value, 0);
                }
                else {
                    _page = Mathf.Clamp(value, 0, info.imageFileURIs.Length-1);
                }
            }
        }

        PDFGenerator gen;
        //PDFImageInfoがロードされたら呼ばれるAction
        PDFImageDirInfo info;


        bool isLoaded;
        RawImage rawImage;

        ///NOWLOADINGの表示
        public GameObject loadingObject;

        void Start()
        {

            gen = new PDFGenerator();
            rawImage = GetComponent<RawImage>();
            Load("samplePDF");
        }

        //矢印キーで次前スライド
        private void Update()
        {
            if (!isLoaded) return;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                page++;
                StartCoroutine(LoadPage(page));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                page--;
                StartCoroutine(LoadPage(page));
            }
        }

        /// <summary>
        /// ロードする
        /// </summary>
        /// <param name="pdfName"></param>
        void Load(string pdfName)
        {
            isLoaded = false;
            page = 0;
            loadingObject.SetActive(true);


            gen.PDFGenerateObservable(pdfName)
                .ObserveOnMainThread()
                .Subscribe(n =>
            {
                info = new PDFImageDirInfo(n);
                loadingObject.SetActive(false);
                isLoaded = true;
                StartCoroutine(LoadPage(0));

            });
        }

        /// <summary>
        /// パスからテクスチャをロードする
        /// </summary>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        IEnumerator LoadPage(int pageCount)
        {
            var uri = "file://" + info.imageFileURIs[pageCount];

            using (var www = new WWW(uri))
            {

                while (!www.isDone)
                {
                    yield return null;
                }

                rawImage.texture = www.texture;
            }
        }


    }
}