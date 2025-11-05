<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        <ThirdPartyFlatFileDetail>
          <!--for system internal use-->
          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>
          <!--for system internal use-->
          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <DCBROKR>
            <xsl:value-of select ="CounterParty"/>
          </DCBROKR>

          <DCTRDBS>
            <xsl:value-of select ="''"/>
          </DCTRDBS>


          <xsl:choose>
            <xsl:when test="Side ='Buy' or Side='Buy to Close' or Side='Buy to Open'">
              <DCBUYSL>
                <xsl:value-of select="'B'"/>
              </DCBUYSL>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell short' or Side='Sell to Open' or Side='Sell to Close'">
              <DCBUYSL>
                <xsl:value-of select="'S'"/>
              </DCBUYSL>
            </xsl:when>
            <xsl:otherwise>
              <DCBUYSL>
                <xsl:value-of select="''"/>
              </DCBUYSL>
            </xsl:otherwise>
          </xsl:choose>


          <DCCXLCR>
            <xsl:value-of select ="''"/>
          </DCCXLCR>

          <DCTRADE>
            <xsl:value-of select="position()"/>
          </DCTRADE>

          <DCACCTN>
            <xsl:value-of select ="FundAccountNo"/>
          </DCACCTN>

          <DCCRRNO>
            <xsl:value-of select ="''"/>
          </DCCRRNO>

          <DCSECDE>
            <xsl:value-of select ="CUSIP"/>
          </DCSECDE>

          <DCMARKT>
            <xsl:value-of select ="''"/>
          </DCMARKT>

          <DCFUNDS>
            <xsl:value-of select="CurrencySymbol"/>
          </DCFUNDS>

          <DCQTY1>
            <xsl:value-of select ="TotalQty"/>
          </DCQTY1>

          <DCPRICE1>
            <xsl:value-of select ="AveragePrice"/>
          </DCPRICE1>

          <DCQTY2>
            <xsl:value-of select ="TotalQty"/>
          </DCQTY2>

          <DCPRICE2>
            <xsl:value-of select ="AveragePrice"/>
          </DCPRICE2>

          <DCQTY3>
            <xsl:value-of select ="TotalQty"/>
          </DCQTY3>

          <DCPRICE3>
            <xsl:value-of select ="AveragePrice"/>
          </DCPRICE3>

          <DCQTY4>
            <xsl:value-of select ="TotalQty"/>
          </DCQTY4>

          <DCPRICE4>
            <xsl:value-of select ="AveragePrice"/>
          </DCPRICE4>

          <DCQTY5>
            <xsl:value-of select ="TotalQty"/>
          </DCQTY5>

          <DCPRICE5>
            <xsl:value-of select ="AveragePrice"/>
          </DCPRICE5>


          <DCCTFRPR>
            <xsl:value-of select ="''"/>
          </DCCTFRPR>

          <DCCBNDI>
            <xsl:value-of select ="''"/>
          </DCCBNDI>

          <DCCCOMM>
            <xsl:value-of select ="CommissionCharged"/>
          </DCCCOMM>

          <DCCGRAM>
            <xsl:value-of select ="GrossAmount"/>
          </DCCGRAM>

          <DCCMOTX>
            <xsl:value-of select ="''"/>
          </DCCMOTX>

          <DCCMQTX>
            <xsl:value-of select ="''"/>
          </DCCMQTX>

          <DCCFQFE>
            <xsl:value-of select ="''"/>
          </DCCFQFE>

          <DCCFOFE>
            <xsl:value-of select ="''"/>
          </DCCFOFE>

          <DCCFAFE>
            <xsl:value-of select ="''"/>
          </DCCFAFE>

          <DCCFBFE>
            <xsl:value-of select ="''"/>
          </DCCFBFE>

          <DCCRGFE>
            <xsl:value-of select ="''"/>
          </DCCRGFE>

          <DCCSCFE>
            <xsl:value-of select ="''"/>
          </DCCSCFE>

          <DCCTSFE>
            <xsl:value-of select ="''"/>
          </DCCTSFE>

          <DCCAINT>
            <xsl:value-of select ="''"/>
          </DCCAINT>

          <DCCPLAM>
            <xsl:value-of select ="''"/>
          </DCCPLAM>

          <DCCEXAM>
            <xsl:value-of select ="''"/>
          </DCCEXAM>

          <DCCPCOMM>
            <xsl:value-of select ="''"/>
          </DCCPCOMM>

          <DCBRINV>
            <xsl:value-of select ="''"/>
          </DCBRINV>

          <DCCEXCR>
            <xsl:value-of select ="''"/>
          </DCCEXCR>

          <DCEXCHI>
            <xsl:value-of select ="''"/>
          </DCEXCHI>

          <DCCTRLR>
            <xsl:value-of select ="''"/>
          </DCCTRLR>

          <DCCSECF>
            <xsl:value-of select ="''"/>
          </DCCSECF>

          <DCCHLDM>
            <xsl:value-of select ="''"/>
          </DCCHLDM>

          <DCVALDT>
            <xsl:value-of select ="SettlementDate"/>
          </DCVALDT>

          <DCTRDDT>
            <xsl:value-of select ="TradeDate"/>
          </DCTRDDT>

          <DCTRLN01>
            <xsl:value-of select ="''"/>
          </DCTRLN01>

          <DCTRLN02>
            <xsl:value-of select ="''"/>
          </DCTRLN02>

          <DCBACCT>
            <xsl:value-of select ="''"/>
          </DCBACCT>

          <DCBRRNO>
            <xsl:value-of select ="''"/>
          </DCBRRNO>
          <xsl:value-of select ="''"/>

          <DCBTRDBS>
            <xsl:value-of select ="''"/>
          </DCBTRDBS>

          <DCBCOMM>
            <xsl:value-of select ="CommissionCharged"/>
          </DCBCOMM>

          <DCBGRAM>
            <xsl:value-of select ="GrossAmount"/>
          </DCBGRAM>

          <DCBMOTX>
            <xsl:value-of select ="''"/>
          </DCBMOTX>

          <DCBMQTX>
            <xsl:value-of select ="''"/>
          </DCBMQTX>

          <DCBFQFE>
            <xsl:value-of select ="''"/>
          </DCBFQFE>

          <DCBFOFE>
            <xsl:value-of select ="''"/>
          </DCBFOFE>

          <DCBFAFE>
            <xsl:value-of select ="''"/>
          </DCBFAFE>

          <DCBFBFE>
            <xsl:value-of select ="''"/>
          </DCBFBFE>

          <DCBRGFE>
            <xsl:value-of select ="''"/>
          </DCBRGFE>

          <DCBSCFE>
            <xsl:value-of select ="''"/>
          </DCBSCFE>

          <DCBTSFE>
            <xsl:value-of select ="''"/>
          </DCBTSFE>

          <DCBAINT>
            <xsl:value-of select ="''"/>
          </DCBAINT>

          <DCBPLAM>
            <xsl:value-of select ="''"/>
          </DCBPLAM>

          <DCBEXAM>
            <xsl:value-of select ="''"/>
          </DCBEXAM>

          <DCBPCOMM>
            <xsl:value-of select ="''"/>
          </DCBPCOMM>

          <DCBEXCR>
            <xsl:value-of select ="''"/>
          </DCBEXCR>

          <DCBEXCHI>
            <xsl:value-of select ="''"/>
          </DCBEXCHI>

          <DCREFNO>
            <xsl:value-of select ="''"/>
          </DCREFNO>

          <DCBTRLN01>
            <xsl:value-of select ="''"/>
          </DCBTRLN01>

          <DCBTRLN02>
            <xsl:value-of select ="''"/>
          </DCBTRLN02>

          <DCBTRLR>
            <xsl:value-of select ="''"/>
          </DCBTRLR>

          <DCBSECF>
            <xsl:value-of select ="''"/>
          </DCBSECF>

          <DCBHLDM>
            <xsl:value-of select ="''"/>
          </DCBHLDM>
          <DC_CC77>
            <xsl:value-of select ="''"/>
          </DC_CC77>

          <DCUSER>
            <xsl:value-of select ="''"/>
          </DCUSER>

          <DCCRTDAT>
            <xsl:value-of select ="''"/>
          </DCCRTDAT>

          <DCPRCDAT>
            <xsl:value-of select ="''"/>
          </DCPRCDAT>

          <!-- system use only-->
          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
