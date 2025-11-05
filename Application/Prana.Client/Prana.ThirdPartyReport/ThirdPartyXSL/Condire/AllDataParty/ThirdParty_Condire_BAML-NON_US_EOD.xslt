<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <msxsl:script language="C#" implements-prefix="my"> public int RoundOff(double Qty) { return (int)Math.Round(Qty,0); } </msxsl:script>

  <xsl:template name="Translate">
    <xsl:param name="Number"/>
    <xsl:variable name="SingleQuote">'</xsl:variable>
    <xsl:variable name="varNumber">
      <xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($Number,'(')">
        <xsl:value-of select="$varNumber*-1"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$varNumber"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select="'False'"/>
        </RowHeader>

        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>

        <Side>
          <xsl:value-of select ="'Side'"/>
        </Side>

        <Ticker>
          <xsl:value-of select="'Ticker'"/>
        </Ticker>

        <CUSIP>
          <xsl:value-of select="'CUSIP'"/>
        </CUSIP>

        <RIC>
          <xsl:value-of select="'RIC'"/>
        </RIC>

        <BBCode>
          <xsl:value-of select="'BBCode'"/>
        </BBCode>

        <ISIN>
          <xsl:value-of select="'ISIN'"/>
        </ISIN>

        <Sedol>
          <xsl:value-of select="'Sedol'"/>
        </Sedol>

        <OrderID>
          <xsl:value-of select ="'OrderID'"/>
        </OrderID>

        <OrderQuantity>
          <xsl:value-of select="'OrderQuantity'"/>
        </OrderQuantity>

        <TradeDate>
          <xsl:value-of select="'TradeDate'"/>
        </TradeDate>

        <SettlementDate>
          <xsl:value-of select ="'SettlementDate'"/>
        </SettlementDate>

        <ExecutionPrice>
          <xsl:value-of select="'ExecutionPrice'"/>
        </ExecutionPrice>

        <ExecutingBrokerCode>
          <xsl:value-of select ="'ExecutingBrokerCode'"/>
        </ExecutingBrokerCode>

        <Account>
          <xsl:value-of select="'Account'"/>
        </Account>

        <TradeCommission>
          <xsl:value-of select="'TradeCommission'"/>
        </TradeCommission>

        <SecFees>
          <xsl:value-of select="'SecFees'"/>
        </SecFees>

        <OtherFees>
          <xsl:value-of select="'OtherFees'"/>
        </OtherFees>

        <StrikePrice>
          <xsl:value-of select="'StrikePrice'"/>
        </StrikePrice>

        <ExpirationDate>
          <xsl:value-of select="'ExpirationDate'"/>
        </ExpirationDate>

        <PutOrCall>
          <xsl:value-of select="'PutOrCall'"/>
        </PutOrCall>

        <UnderlyingSymbol>
          <xsl:value-of select="'UnderlyingSymbol'"/>
        </UnderlyingSymbol>

        <Exchange>
          <xsl:value-of select="'Exchange'"/>
        </Exchange>

        <TradedCurrency>
          <xsl:value-of select="'TradedCurrency'"/>
        </TradedCurrency>

        <PB>
          <xsl:value-of select="'PB'"/>
        </PB>

        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>

      <xsl:for-each select="ThirdPartyFlatFileDetail[CurrencySymbol!='USD']">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select="'False'"/>
          </RowHeader>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

          <xsl:choose>
            <xsl:when test="Side='Buy to Open' or Side='Buy' ">
              <Side>
                <xsl:value-of select ="'B'"/>
              </Side>
            </xsl:when>
            <xsl:when test="Side='Buy to Cover' or Side='Buy to Close' ">
              <Side>
                <xsl:value-of select ="'BTC'"/>
              </Side>
            </xsl:when>
            <xsl:when test="Side='Sell' or Side='Sell to Close' ">
              <Side>
                <xsl:value-of select ="'S'"/>
              </Side>
            </xsl:when>
            <xsl:when test="Side='Sell short' or Side='Sell to Open' ">
              <Side>
                <xsl:value-of select ="'SS'"/>
              </Side>
            </xsl:when>
            <xsl:otherwise>
              <Side>
                <xsl:value-of select="Side"/>
              </Side>
            </xsl:otherwise>
          </xsl:choose>

          <Ticker>
            <xsl:value-of select="Symbol"/>
          </Ticker>

          <CUSIP>
            <xsl:value-of select="CUSIPSymbol"/>
          </CUSIP>

          <RIC>
            <xsl:value-of select="RIC"/>
          </RIC>

          <BBCode>
            <xsl:value-of select="BBCode"/>
          </BBCode>

          <ISIN>
            <xsl:value-of select="ISIN"/>
          </ISIN>

          <Sedol>
            <xsl:value-of select="SEDOL"/>
          </Sedol>

          <OrderID>
            <xsl:value-of select ="TradeRefID"/>
          </OrderID>

          <OrderQuantity>
            <xsl:value-of select="AllocatedQty"/>
          </OrderQuantity>

          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

          <SettlementDate>
            <xsl:value-of select ="SettlementDate"/>
          </SettlementDate>

          <ExecutionPrice>
            <xsl:value-of select="AveragePrice"/>
          </ExecutionPrice>

          <ExecutingBrokerCode>
            <xsl:value-of select ="CounterParty"/>
          </ExecutingBrokerCode>

          <xsl:choose>
            <xsl:when test="AccountName='CRPR-MS-PB'">
              <Account>
                <xsl:value-of select="'038CDOQE6'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='CRPL-MS-PB'">
              <Account>
                <xsl:value-of select="'038CACAH5'"/>
              </Account>
            </xsl:when>


            <xsl:when test="AccountName='CRPR-MS-SWAP'">
              <Account>
                <xsl:value-of select="'06178YD96'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='CRPL-MS-SWAP'">
              <Account>
                <xsl:value-of select="'06178Y5A2'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='CRPI-MS-PB'">
              <Account>
                <xsl:value-of select="'038CAD183'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='CRPI-MS-SWAP'">
              <Account>
                <xsl:value-of select="'06178JZ20'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='ST-Main-MS-PB'">
              <Account>
                <xsl:value-of select="'038CACXV9'"/>
              </Account>
            </xsl:when>
            <xsl:when test="AccountName='ST-Main-MS-SWAP1'">
              <Account>
                <xsl:value-of select="'0617847X6'"/>
              </Account>
            </xsl:when>
            <xsl:when test="AccountName='ST-Main-MS-SWAP2'">
              <Account>
                <xsl:value-of select="'0617847X6'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='TO-DAWS-MS-PB'">
              <Account>
                <xsl:value-of select="'038CDKU41'"/>
              </Account>
            </xsl:when>
            <xsl:when test="AccountName='ST-PUFF-MS-PB'">
              <Account>
                <xsl:value-of select="'038CACXT4'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='ST-MONT-MS-PB'">
              <Account>
                <xsl:value-of select="'038CACXU1'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='ST-MONT-MS-SWAP'">
              <Account>
                <xsl:value-of select="'0617847Y4'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='ST-PUFF-MS-SWAP'">
              <Account>
                <xsl:value-of select="'0617847Z1'"/>
              </Account>
            </xsl:when>
            <xsl:when test="AccountName='TO-ELKO-MS-PB'">
              <Account>
                <xsl:value-of select="'038CDKTN1'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='ST-EDV-MS-PB'">
              <Account>
                <xsl:value-of select="'038CADTV2'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='ST-EDV-MS-SWAP'">
              <Account>
                <xsl:value-of select="'06178JQ20'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='CRPA-MS-PB'">
              <Account>
                <xsl:value-of select="'038CAEH84'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='CRPA-MS-SWAP'">
              <Account>
                <xsl:value-of select="'038CAEH84'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='CRPA-JEFF-PB'">
              <Account>
                <xsl:value-of select="'43300610'"/>
              </Account>
            </xsl:when>

            <xsl:when test="AccountName='CRPL-JEFF-PB'">
              <Account>
                <xsl:value-of select="'43300612'"/>
              </Account>
            </xsl:when>
            <xsl:when test="AccountName='CRPI-JEFF-PB'">
              <Account>
                <xsl:value-of select="'43300613'"/>
              </Account>
            </xsl:when>
            <xsl:when test="AccountName='CRPR-JEFF-PB'">
              <Account>
                <xsl:value-of select="'43300609'"/>
              </Account>
            </xsl:when>

            <xsl:otherwise>
              <Account>
                <xsl:value-of select="AccountName"/>
              </Account>
            </xsl:otherwise>
          </xsl:choose>


          <TradeCommission>
            <xsl:value-of select="CommissionCharged"/>
          </TradeCommission>


          <SecFees>
            <xsl:value-of select="Secfee + StampDuty"/>
          </SecFees>

          <OtherFees>
            <xsl:value-of select="TaxOnCommissions + TransactionLevy + ClearingFee + MiscFees + OtherBrokerFee"/>
          </OtherFees>

          <StrikePrice>
            <xsl:value-of select="StrikePrice"/>
          </StrikePrice>

          <ExpirationDate>
            <xsl:value-of select="ExpirationDate"/>
          </ExpirationDate>

          <PutOrCall>
            <xsl:value-of select="PutOrCall"/>
          </PutOrCall>

          <UnderlyingSymbol>
            <xsl:value-of select="UnderlyingSymbol"/>
          </UnderlyingSymbol>

          <Exchange>
            <xsl:value-of select="Exchange"/>
          </Exchange>

          <TradedCurrency>
            <xsl:value-of select="CurrencySymbol"/>
          </TradedCurrency>

          <xsl:choose>
            <xsl:when test="AccountName='CRPR-MS-PB' or AccountName='CRPL-MS-PB' or AccountName='CRPR-MS-SWAP' or AccountName='CRPL-MS-SWAP' or AccountName='CRPI-MS-PB' or AccountName='CRPI-MS-SWAP' or AccountName='ST-Main-MS-PB' or AccountName='ST-Main-MS-SWAP1' or AccountName='ST-Main-MS-SWAP2' or AccountName='TO-DAWS-MS-PB' or AccountName='ST-PUFF-MS-PB' or AccountName='ST-MONT-MS-PB' or AccountName='ST-MONT-MS-SWAP' or AccountName='ST-PUFF-MS-SWAP' or AccountName='TO-ELKO-MS-PB' or AccountName='ST-EDV-MS-PB' or AccountName='ST-EDV-MS-SWAP' or AccountName='CRPA-MS-PB' or AccountName='CRPA-MS-SWAP'">
              <PB>
                <xsl:value-of select="'MS'"/>
              </PB>
            </xsl:when>

            <xsl:when test="AccountName='CRPA-JEFF-PB' or AccountName='CRPL-JEFF-PB' or AccountName='CRPI-JEFF-PB' or AccountName='CRPR-JEFF-PB'">
              <PB>
                <xsl:value-of select="'Jefferies'"/>
              </PB>
            </xsl:when>


            <xsl:otherwise>
              <PB>
                <xsl:value-of select="''"/>
              </PB>
            </xsl:otherwise>
          </xsl:choose>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>

      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>

  <xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>