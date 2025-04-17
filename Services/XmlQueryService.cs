using MyPyramidWeb.Abstractions;

namespace MyPyramidWeb.Services;

public class XmlQueryService : IXmlQueryService
{
    public string GetQueryFromEntitiesStep1(string subject, string tu)
    {
        string xmlQuery =
            $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://www.sicon.ru/Integration/Pyramid/2019/08"" xmlns:arr=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
      <soapenv:Header/>
      <soapenv:Body>
         <ns:RequestEntities>
            <ns:message>
               <ns:Header>
                  <ns:Source>{subject}</ns:Source>
               </ns:Header>
               <ns:EntityIds>
                  <arr:string>{tu}</arr:string>
               </ns:EntityIds>
               <ns:Filters>
                  <ns:Filter/>
               </ns:Filters>
            </ns:message>
         </ns:RequestEntities> 
      </soapenv:Body>
   </soapenv:Envelope>";

        return xmlQuery;
    }

    public string GetQueryFromEntitiesStep2(string subject, int requestId)
    {
        string xmlQuery =
            @$"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://www.sicon.ru/Integration/Pyramid/2019/08"">
         <soapenv:Header/>
         <soapenv:Body>
               <ns:FetchEntities>
                  <!--Optional:-->
                  <ns:message>
                     <!--Optional:-->
                     <ns:Header>
                     <!--Optional:-->
                     <ns:Source>{subject}</ns:Source>
                     </ns:Header>
                     <!--Optional:-->
                     <ns:RequestId>{requestId}</ns:RequestId>
                  </ns:message>
               </ns:FetchEntities>
         </soapenv:Body>
         </soapenv:Envelope>";

        return xmlQuery;
    }
}