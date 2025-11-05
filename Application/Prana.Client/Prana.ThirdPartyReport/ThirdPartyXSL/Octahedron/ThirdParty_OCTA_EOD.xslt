<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'false'"/>
        </RowHeader>


        <TaxLotState>
          <xsl:value-of select="TaxLotState"/>
        </TaxLotState>


        <Order>
          <xsl:value-of select="'Order'"/>
        </Order>
        
        <Acct>
          <xsl:value-of select="'Acct'"/>
        </Acct>

        <AllocAcct>
          <xsl:value-of select="'AllocAcct'"/>
        </AllocAcct>

      
        
        <PrintSymbol>
          <xsl:value-of select="'PrintSymbol'"/>
        </PrintSymbol>

        <Side>
          <xsl:value-of select="'Side'"/>
        </Side>

        <Qty>
          <xsl:value-of select="'Qty'"/>
        </Qty>

        <Filled>
          <xsl:value-of select="'Filled'"/>
        </Filled>

        <CMTA>
          <xsl:value-of select="'CMTA'"/>
        </CMTA>

        <Leaves>
          <xsl:value-of select="'Leaves'"/>
        </Leaves>

        <Px>
          <xsl:value-of select="'Px'"/>
        </Px>

        <AvgPx>
          <xsl:value-of select="'AvgPx'"/>
        </AvgPx>

        <TIF>
          <xsl:value-of select="'TIF'"/>
        </TIF>

        <Status>
          <xsl:value-of select="'Status'"/>
        </Status>

        <Dest>
          <xsl:value-of select="'Dest'"/>
        </Dest>

        <Instructions>
          <xsl:value-of select="'Instructions'"/>
        </Instructions>

        <Comment>
          <xsl:value-of select="'Comment'"/>
        </Comment>

        <clientComm>
          <xsl:value-of select="'clientComm'"/>
        </clientComm>


        <execComm>
          <xsl:value-of select="'execComm'"/>
        </execComm>

        <brokerComm>
          <xsl:value-of select="'brokerComm'"/>
        </brokerComm>

       
        <softComm>
          <xsl:value-of select="'softComm'"/>
        </softComm>

        <Open>
          <xsl:value-of select="'Open'"/>
        </Open>


        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>


      </ThirdPartyFlatFileDetail>
      <xsl:for-each select="ThirdPartyFlatFileDetail">

        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'false'"/>
          </RowHeader>


          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>


          <Order>
            <xsl:value-of select="PBUniqueID"/>
          </Order>
          <xsl:variable name="PB_NAME" select="''"/>

          <xsl:variable name = "PRANA_FUND_NAME">
            <xsl:value-of select="AccountName"/>
          </xsl:variable>

          <xsl:variable name ="THIRDPARTY_FUND_CODE">
            <xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
          </xsl:variable>
          <Acct>
            <xsl:value-of select="'OCTA'"/>
          </Acct>

          <AllocAcct>
            <xsl:choose>
              <xsl:when test="$THIRDPARTY_FUND_CODE!=''">
                <xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$PRANA_FUND_NAME"/>
              </xsl:otherwise>
            </xsl:choose>
          </AllocAcct>

          <xsl:variable name="varSymbol">
            <xsl:choose>
              <xsl:when test="IsSwapped='true'">
                <xsl:value-of select="concat(Symbol,'.SWAP')"/>
              </xsl:when>
              <xsl:when test="Asset='EquityOption'">
                <xsl:value-of select="FullSecurityName"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Symbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <PrintSymbol>
            <xsl:value-of select="$varSymbol"/>
          </PrintSymbol>

          <Side>
            <xsl:choose>
              <xsl:when test="Side='Buy'">
                <xsl:value-of select="'Buy'"/>
              </xsl:when>
              <xsl:when test="Side='Buy to Close'">
                <xsl:value-of select="'Cover'"/>
              </xsl:when>
              <xsl:when test="Side='Sell short'">
                <xsl:value-of select="'Short'"/>
              </xsl:when>
              <xsl:when test="Side='Sell'">
                <xsl:value-of select="'Sell'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Side"/>
              </xsl:otherwise>
            </xsl:choose>
          </Side>

          <Qty>
            <xsl:value-of select="TotalQty"/>
          </Qty>

          <Filled>
            <xsl:value-of select="ExecutedQty"/>
          </Filled>

          <CMTA>
            <xsl:value-of select="''"/>
          </CMTA>

          <!--<xsl:variable name="varLeaves">
            <xsl:value-of select="((AllocatedQty div ExecutedQty) * TotalQty) - AllocatedQty"/>
          </xsl:variable>-->

          <xsl:variable name="varLeaves">
            <xsl:value-of select="(TotalQty - ExecutedQty)"/>
          </xsl:variable>
          <Leaves>
            <xsl:value-of select="$varLeaves"/>
          </Leaves>

          <Px>
            <xsl:value-of select="'MKT'"/>
          </Px>

          <AvgPx>
            <xsl:value-of select="AveragePrice"/>
          </AvgPx>

          <TIF>
            <xsl:value-of select="0"/>
          </TIF>

          <Status>
            <xsl:choose>
              <xsl:when test="$varLeaves !=0">
                <xsl:value-of select="'PARTILY FILLED'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'FILLED'"/>
              </xsl:otherwise>
            </xsl:choose>

          </Status>

          <Dest>
            <xsl:value-of select="'JONE'"/>
          </Dest>

          <Instructions>
            <xsl:value-of select="''"/>
          </Instructions>

          <Comment>
            <xsl:value-of select="''"/>
          </Comment>


          <xsl:variable name="varCommRateBps">
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="format-number(((CommissionCharged * 10000) div (AllocatedQty * AveragePrice)),'##0.000')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>


          <xsl:variable name="varCommTotal">
            <xsl:value-of select="(CommissionCharged)+(SoftCommissionCharged)"/>
          </xsl:variable>
          <xsl:variable name="varCommRate">
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="$varCommTotal div (AllocatedQty)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <clientComm>
            <xsl:choose>
              <xsl:when test="Exchange='Basket Swap' and IsSwapped='true'">
                <xsl:value-of select="concat($varCommRateBps,'bps')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="format-number($varCommRate,'##.####')"/>
              </xsl:otherwise>
            </xsl:choose>
          </clientComm>




          <xsl:variable name="varExecCommBps">
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="(CommissionCharged * 10000) div (AllocatedQty * AveragePrice)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varExecCommRate">
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="CommissionCharged div (AllocatedQty)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <execComm>
            <xsl:choose>
              <xsl:when test="Exchange='Basket Swap' and IsSwapped='true'">
                <xsl:value-of select="0"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="format-number($varExecCommRate,'##.####')"/>
              </xsl:otherwise>
            </xsl:choose>

          </execComm>

          <brokerComm>
            <xsl:choose>
              <xsl:when test="Exchange='Basket Swap' and IsSwapped='true'">
                <xsl:value-of select="concat($varCommRateBps,'bps')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </brokerComm>

          <xsl:variable name="varSoftCommBps">
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="(SoftCommissionCharged * 10000) div (AllocatedQty * AveragePrice)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="varSoftCommRate">
            <xsl:choose>
              <xsl:when test="number(AllocatedQty)">
                <xsl:value-of select="SoftCommissionCharged div (AllocatedQty)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <softComm>
            <xsl:choose>
              <xsl:when test="Exchange='Basket Swap' and IsSwapped='true'">
                <xsl:value-of select="concat($varSoftCommBps,'bps')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="format-number($varSoftCommRate,'##.####')"/>
              </xsl:otherwise>
            </xsl:choose>
          </softComm>

          <Open>
            <xsl:value-of select="TradeDateTime"/>
          </Open>


          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>


        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
