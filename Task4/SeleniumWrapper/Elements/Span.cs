using System;
using OpenQA.Selenium;

namespace SeleniumWrapper.Elements
{
    public sealed class Span : BaseElement
    {
        public Span(By by, int ind, BaseElement parentElemen) : base(by,ind, parentElemen)
        {
        }
    }
}