<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">

        <xsl:if test ="number(COL7) and COL32!='CURRENCY'">

          <xsl:variable name="varPBName">
            <xsl:value-of select="'Cowen'"/>
          </xsl:variable>

          <PositionMaster>

            <xsl:variable name="PB_Symbol">
              <xsl:value-of select = "COL26"/>
            </xsl:variable>
            <xsl:variable name="PRANA_SYMBOL_NAME">
              <xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
            </xsl:variable>
            
             <xsl:variable name="varISIN">
              <xsl:value-of select="normalize-space(COL22)"/>
            </xsl:variable>
            <Symbol>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                </xsl:when>
				        <xsl:when test="$varISIN !='' or $varISIN !='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="PB_Symbol"/>
                </xsl:otherwise>
              </xsl:choose>
            </Symbol>


            <ISIN>
              <xsl:choose>
                <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                  <xsl:value-of select="''"/>
                </xsl:when>
                <xsl:when test="$varISIN !='' or $varISIN !='*'">
                  <xsl:value-of select="$varISIN"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </ISIN>


			  <AccountName>
				  <xsl:value-of select="'SSP Fund Cowen PHW006826'"/>
			  </AccountName>


            <xsl:variable name="varAsset">
              <xsl:value-of select="normalize-space(COL32)"/>
            </xsl:variable>
            <Asset>
				<xsl:choose>
					<xsl:when test="$varAsset='COMMON STOCK'">
						<xsl:value-of select="'Equity'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
            </Asset>
            
            <xsl:variable name="varTradeDate">
            	<xsl:value-of select="COL28"/>
            </xsl:variable>
			  
            <TradeDate>
              <xsl:value-of select="$varTradeDate"/>
            </TradeDate>
            
            
            <xsl:variable name="varSettlementDate">
				<xsl:value-of select="COL27"/>
            </xsl:variable>
            <SettlementDate>
              <xsl:value-of select="$varSettlementDate"/>
            </SettlementDate>
            

            <xsl:variable name="varQuantity">
              <xsl:value-of select="COL7"/>
            </xsl:variable>
            <Quantity>
			         <xsl:choose>
                <xsl:when test="$varQuantity &gt; 0">
                  <xsl:value-of select="$varQuantity"/>
                </xsl:when>
                <xsl:when test="$varQuantity &lt; 0">
                  <xsl:value-of select="$varQuantity*(-1)"/>
                </xsl:when>
			        </xsl:choose>
            </Quantity>
            
            
            <xsl:variable name="varAvgPX">
               <xsl:value-of select="COL8"/>
            </xsl:variable>
			  <AvgPX>
				  <xsl:value-of select="$varAvgPX"/>
			  </AvgPX>
            
            
            <xsl:variable name="varCommission">
              <xsl:value-of select="COL10"/>
            </xsl:variable>
            <Commission>
			         <xsl:choose>
                <xsl:when test="$varCommission &gt; 0">
                  <xsl:value-of select="$varCommission"/>
                </xsl:when>
                <xsl:when test="$varCommission &lt; 0">
                  <xsl:value-of select="$varCommission*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
			        </xsl:choose>
            </Commission>
            
            
            <xsl:variable name="varAUECFee1">
               <xsl:value-of select="COL11"/>
            </xsl:variable>
            <AUECFee1>
              <xsl:choose>
                <xsl:when test="$varAUECFee1 &gt; 0">
                  <xsl:value-of select="$varAUECFee1"/>
                </xsl:when>
                <xsl:when test="$varAUECFee1 &lt; 0">
                  <xsl:value-of select="$varAUECFee1*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
			        </xsl:choose>
            </AUECFee1>
            

            <xsl:variable name="varNetNotionalValue">
              <xsl:value-of select="COL25"/>
            </xsl:variable>
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValue &gt; 0">
                  <xsl:value-of select="$varNetNotionalValue"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValue &lt; 0">
                  <xsl:value-of select="$varNetNotionalValue*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <xsl:variable name="varNetNotionalValueBase">
              <xsl:value-of select="COL12"/>
            </xsl:variable>
            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test="$varNetNotionalValueBase &gt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>
                <xsl:when test="$varNetNotionalValueBase &lt; 0">
                  <xsl:value-of select="$varNetNotionalValueBase*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>


           <xsl:variable name="varSide">
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>
            <Side>
              <xsl:choose>
                <xsl:when test="$varSide='BUY'">
                  <xsl:value-of select="'Buy'"/>
                </xsl:when>
                <xsl:when test="$varSide='SELL'">
                  <xsl:value-of select="'Sell'"/>
                </xsl:when>
              </xsl:choose>
            </Side>
          
            <xsl:variable name="varSettlCurrency">
              <xsl:value-of select="normalize-space(COL24)"/>
            </xsl:variable>
            <SettlCurrency>
              <xsl:value-of select="$varSettlCurrency"/>
            </SettlCurrency>

            
            <xsl:variable name="varCurrency">
              <xsl:value-of select="normalize-space(COL17)"/>
            </xsl:variable>
            <BaseCurrency>
              <xsl:value-of select="$varCurrency"/>
            </BaseCurrency>
            
            <SMRequest>
              <xsl:value-of select="'TRUE'"/>
            </SMRequest>
        
          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>