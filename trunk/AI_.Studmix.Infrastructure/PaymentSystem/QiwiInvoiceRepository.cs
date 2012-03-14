using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using AI_.Studmix.Domain.Entities;
using AI_.Studmix.Domain.Repository;
using AI_.Studmix.Domain.Services.Abstractions;

namespace AI_.Studmix.Infrastructure.PaymentSystem
{
    public class QiwiInvoiceRepository : IPaymentSystemInvoiceRepository
    {
        private static readonly Encoding MDefaultEncoding = new UTF8Encoding(false);
        protected IPaymentSystmeProviderConfiguration Configuration { get; set; }

        public QiwiInvoiceRepository(IPaymentSystmeProviderConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region IPaymentSystemInvoiceRepository Members

        public void StoreInvoice(Invoice invoice)
        {
            var request = BuildCreateBillRequest(invoice);

            var response = Post(request);
            var resultCode = response.Element("result-code").Value;
            if (resultCode != "0")
            {
                throw new ProviderException("Qiwi service request error. ResultCode: " + resultCode);
            }
        }

        public InvoiceStatus GetInvoiceStatus(Invoice invoice)
        {
            var request = BuildGetBillStatusRequest(new List<Invoice> {invoice});

            var response = Post(request);
            var resultCode = response.Element("bills-list").Element("bill").Attribute("status").Value;
            return ConvertToStatus(int.Parse(resultCode));
        }

        public IDictionary<Guid, InvoiceStatus> GetInvoiceStatuses(IEnumerable<Invoice> invoices)
        {
            var request = BuildGetBillStatusRequest(invoices);

            var response = Post(request);
            var resultCode = response.Element("bills-list").Element("bill").Attribute("status").Value;
            return response.Element("bills-list").Elements("bill")
                .ToDictionary(e => Guid.Parse(e.Attribute("id").Value),
                              e => ConvertToStatus(int.Parse(e.Attribute("status").Value)));
        }

        #endregion

        private InvoiceStatus ConvertToStatus(int code)
        {
            if (code < 60)
            {
                return InvoiceStatus.Invoiced;
            }
            if (code == 60)
            {
                return InvoiceStatus.Paid;
            }
            if (code >= 100)
            {
                return InvoiceStatus.Canceled;
            }
            throw new ArgumentOutOfRangeException();
        }

        private XElement BuildCreateBillRequest(Invoice invoice)
        {
            var request = new XElement("request");
            request.Add(new XElement("request-type", 30));
            request.Add(new XElement("terminal-id", Configuration.Login));
            request.Add(GetExtraAttribute("password", Configuration.Password));
            request.Add(GetExtraAttribute("to-account", invoice.User.PhoneNumber));
            request.Add(GetExtraAttribute("amount", invoice.Amount.ToString()));
            request.Add(GetExtraAttribute("ALARM_SMS", "0"));
            request.Add(GetExtraAttribute("ACCEPT_CALL", "0"));
            request.Add(GetExtraAttribute("create-agt", "1"));
            request.Add(GetExtraAttribute("trm-id", invoice.TransactionID.ToString()));
            request.Add(GetExtraAttribute("comment", invoice.Comment));
            return request;
        }

        private XElement BuildGetBillStatusRequest(IEnumerable<Invoice> invoices)
        {
            var request = new XElement("request");
            request.Add(new XElement("request-type", 33));
            request.Add(new XElement("terminal-id", Configuration.Login));
            request.Add(GetExtraAttribute("password", Configuration.Password));
            var billList = new XElement("bills-list");
            request.Add(billList);
            foreach (var invoice in invoices)
            {
                var billElement = new XElement("bill");
                billElement.Add(new XAttribute("txn-id", invoice.TransactionID));
                billList.Add(billElement);
            }
            return request;
        }

        private XElement GetExtraAttribute(string name, string value)
        {
            var extra = new XElement("extra");
            extra.Add(new XAttribute("name", name));
            extra.Value = value;
            return extra;
        }


        private XElement Post(XElement request)
        {
            HttpWebRequest webRequest = SendRequest(request, Configuration.Url);
            return ReceiveResponse(webRequest);
        }

        private HttpWebRequest SendRequest(XElement request, string url)
        {
            HttpWebRequest httpwebRequest;
            Stream requestStream = null;
            try
            {
                var content = MDefaultEncoding.GetBytes(request.ToString());

                httpwebRequest = (HttpWebRequest) WebRequest.Create(url);
                httpwebRequest.Method = "POST";
                httpwebRequest.ContentType = "application/x-www-form-urlencoded";
                httpwebRequest.Timeout = 20000;
                httpwebRequest.ContentLength = content.Length;
                requestStream = httpwebRequest.GetRequestStream();
                requestStream.Write(content, 0, content.Length);
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();
            }
            return httpwebRequest;
        }

        private XElement ReceiveResponse(HttpWebRequest httpwebRequest)
        {
            StreamReader responseStream = null;
            try
            {
                var httpwebResponse = (HttpWebResponse) httpwebRequest.GetResponse();
                responseStream = new StreamReader(httpwebResponse.GetResponseStream(), MDefaultEncoding);
                return XElement.Parse(responseStream.ReadToEnd());
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Close();
            }
        }
    }
}