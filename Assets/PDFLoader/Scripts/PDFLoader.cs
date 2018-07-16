using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

namespace uPDFLoader
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
        int currentPage
        {
            get { return _page; }
            set
            {
                if (info == null)
                {
                    _page = Mathf.Max(value, 0);
                }
                else
                {
                    _page = Mathf.Clamp(value, 0, info.imageFileURIs.Length - 1);
                }
            }
        }

        PDFGenerator gen;
        //PDFImageInfoがロードされたら呼ばれるAction
        PDFImageDirInfo info;


        ReactiveProperty<bool> isPDFLoadedProperty = new ReactiveProperty<bool>();
        RawImage rawImage;

        ///NOWLOADINGの表示
        public GameObject loadingObject;

        void Start()
        {
            gen = new PDFGenerator();
            rawImage = GetComponent<RawImage>();
            Load("samplePDF");

            isPDFLoadedProperty.ObserveOnMainThread().Subscribe(loaded =>
            {
                if (loaded)
                {
                    //PDFロードが完了した時
                    StartCoroutine(LoadPage(0));
                    loadingObject.SetActive(false);

                }
                else
                {
                    //初期化された時
                    currentPage = 0;
                    loadingObject.SetActive(true);
                    info = null;
                }
            });
        }

        //矢印キーで次前スライド
        private void Update()
        {
            if (!isPDFLoadedProperty.Value) return;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentPage++;
                StartCoroutine(LoadPage(currentPage));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentPage--;
                StartCoroutine(LoadPage(currentPage));
            }
        }

        /// <summary>
        /// ロードする
        /// </summary>
        /// <param name="pdfName"></param>
        void Load(string pdfName)
        {
            isPDFLoadedProperty.Value = false;

            gen.PDFGenerateObservable(pdfName)
                .Subscribe(n =>
                {
                    info = new PDFImageDirInfo(n);
                    isPDFLoadedProperty.Value = true;
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