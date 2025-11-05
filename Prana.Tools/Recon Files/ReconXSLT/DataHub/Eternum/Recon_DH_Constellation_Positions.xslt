<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="GetSuffix">
    <xsl:param name="Suffix"/>
    <xsl:choose>
      <xsl:when test="$Suffix = 'JPY'">
        <xsl:value-of select="'-TSE'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CHF'">
        <xsl:value-of select="'-SWX'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'EUR'">
        <xsl:value-of select="'-EEB'"/>
      </xsl:when>
      <xsl:when test="$Suffix = 'CAD'">
        <xsl:value-of select="'-TC'"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month=01 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=02 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=03 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=04 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=05 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=06 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=07  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=08  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=09 ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month=10 ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month=11 ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month=12 ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:param name="Suffix"/>
		<xsl:if test="COL6='Options'">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL4),' '),' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL4),'/'),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),' '),' '),'/')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-after(substring-after(normalize-space(COL4),'/'),'/'),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(normalize-space(COL4),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(normalize-space(COL4),' '),' '),' '),' '),'#.00')"/>

			</xsl:variable>

			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthCode">
					<xsl:with-param name="Month" select="number($ExpiryMonth)"/>
					<xsl:with-param name="PutOrCall" select="$PutORCall"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="Day">
				<xsl:choose>
					<xsl:when test="substring($ExpiryDay,1,1)='0'">
						<xsl:value-of select="substring($ExpiryDay,2,1)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$ExpiryDay"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>			
			<xsl:value-of select="concat('O:',$UnderlyingSymbol,$Suffix,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>			
		</xsl:if>
	</xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:choose>
        <xsl:when test="number(COL8) and normalize-space(COL6) != 'Cash and Equivalents'">

          <PositionMaster>
            <xsl:variable name="varPBName">
              <xsl:value-of select="'Jeff'"/>
            </xsl:variable>

            <xsl:variable name = "PB_FUND_NAME">
              <xsl:value-of select="COL3"/>
            </xsl:variable>

            <xsl:variable name="PRANA_FUND_NAME">
              <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
            </xsl:variable>

            <xsl:variable name = "PB_Symbol_NAME" >
              <xsl:value-of select="normalize-space(COL4)"/>
            </xsl:variable>

            <xsl:variable name="PRANA_Symbol_NAME">
              <xsl:value-of select="document('../../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
            </xsl:variable>

            <xsl:variable name="varPositionStartDate">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varQuantity">
              <xsl:value-of select="COL8"/>
            </xsl:variable>

            <xsl:variable name="varOptionSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varEquitySymbol">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="varFutureSymbol">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varCUSIP">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varRIC">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varBloomberg">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varSEDOL">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varOSISymbol">
              <xsl:value-of select="COL19"/>
            </xsl:variable>

            <xsl:variable name="varOptionExpiry">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varPBSymbol">
              <xsl:value-of select="COL2"/>
            </xsl:variable>

            <xsl:variable name="CompanyName">
              <xsl:value-of select="COL4"/>
            </xsl:variable>

            <xsl:variable name="varMarkPrice">
              <xsl:value-of select="COL13"/>
            </xsl:variable>
            <xsl:variable name="varNetNotionalValue">
              <xsl:value-of select="COL11"/>
            </xsl:variable>

            <xsl:variable name="varNetNotionalValueBase">
              <xsl:value-of select="COL10"/>
            </xsl:variable>

            <xsl:variable name="varCounterPartyID">
              <xsl:value-of select="''"/>
            </xsl:variable>

            <xsl:variable name="varAssetType">
              <xsl:value-of select="normalize-space(COL6)"/>
            </xsl:variable>

            <xsl:variable name="varCommission">
              <xsl:value-of select="0"/>
            </xsl:variable>

            <xsl:variable name="varMarketValue">
              <xsl:value-of select="COL17"/>
            </xsl:variable>

            <xsl:variable name="varMarketValueBase">
              <xsl:value-of select="COL16"/>
            </xsl:variable>

            <xsl:variable name="varSMRequest">
              <xsl:value-of select="'true'"/>
            </xsl:variable>

                <FundName>
                  <xsl:choose>
                    <xsl:when test="$PRANA_FUND_NAME=''">
                  <xsl:value-of select="$PB_FUND_NAME"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$PRANA_FUND_NAME"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </FundName>

            <PositionStartDate>
              <xsl:value-of select="$varPositionStartDate"/>
            </PositionStartDate>

            <xsl:variable name="varSuffixValue">
              <xsl:choose>
                <xsl:when test="$varAssetType = 'Equity' or $varAssetType = 'Fixed Income'">
                  <xsl:value-of  select="COL6"/>
                </xsl:when>
                <xsl:when test="$varAssetType = 'EQUITY'">
                  <xsl:value-of select="substring-after($varEquitySymbol, '.')"/>
                </xsl:when>
              </xsl:choose>
                </xsl:variable>
              
                <xsl:variable name="varSuffix">
                  <xsl:call-template name="GetSuffix">
                    <xsl:with-param name="Suffix" select="$varSuffixValue"/>
                  </xsl:call-template>
                </xsl:variable>
                <Symbol>
                  <xsl:choose>
                    <xsl:when test="$PRANA_Symbol_NAME != ''">
                      <xsl:value-of select="$PRANA_Symbol_NAME"/>
                    </xsl:when>
					  
                    <xsl:when test="$varAssetType='Options'">						
							<xsl:call-template name="Option">
								<xsl:with-param name="Symbol" select="COL19"/>
							</xsl:call-template>
					</xsl:when>
                    
                    <xsl:when test="COL5 != 'USD'">
                       <xsl:value-of select="''"/>
                    </xsl:when>
                     <xsl:when test="contains($varEquitySymbol, '.') != false">
                        <xsl:value-of select="concat(substring-before($varEquitySymbol, '.'), $varSuffix)"/>
                      </xsl:when>
                      <xsl:otherwise>
                      <xsl:value-of select="$varEquitySymbol"/>
                       </xsl:otherwise>
                      </xsl:choose>
                </Symbol>
                
               
                
                <!--<SEDOL>
                  <xsl:choose>
                    <xsl:when test="$PRANA_Symbol_NAME != ''">
                      <xsl:value-of select="''"/>
                    </xsl:when>
                    <xsl:when test="$varAssetType='Options'">
                      <xsl:value-of select="''"/>
                    </xsl:when>
                        <xsl:when test="COL5 != 'USD'">
                          <xsl:value-of select="normalize-space(COL2)"/>
                        </xsl:when>
                        <xsl:when test="contains($varEquitySymbol, '.') != false">
                          <xsl:value-of select="''"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="''"/>
                        </xsl:otherwise>
                  </xsl:choose>
                </SEDOL>-->

            <PBSymbol>
              <xsl:value-of select="$varPBSymbol"/>
            </PBSymbol>

            <CompanyName>
              <xsl:value-of select="$CompanyName"/>
            </CompanyName>

            <Quantity>
              <xsl:value-of select="$varQuantity"/>
            </Quantity>

            <MarkPrice>
              <xsl:choose>
                <xsl:when test ="boolean(number($varMarkPrice))">
                  <xsl:value-of select="$varMarkPrice"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarkPrice>

            <MarketValue>
              <xsl:choose>
                <xsl:when test ="number($varMarketValue) ">
                  <xsl:value-of select="$varMarketValue"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValue>

            <MarketValueBase>
              <xsl:choose>
                <xsl:when test ="number($varMarketValueBase) ">
                  <xsl:value-of select="$varMarketValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MarketValueBase>
                
            <NetNotionalValue>
              <xsl:choose>
                <xsl:when test ="number($varNetNotionalValue) ">
                  <xsl:value-of select="$varNetNotionalValue"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValue>

            <NetNotionalValueBase>
              <xsl:choose>
                <xsl:when test ="number($varNetNotionalValueBase) ">
                  <xsl:value-of select="$varNetNotionalValueBase"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </NetNotionalValueBase>

            <CUSIPSymbol>
              <xsl:value-of select="COL20"/>
            </CUSIPSymbol>

            <!--<SEDOLSymbol>
              <xsl:value-of select="COL19"/>
            </SEDOLSymbol>-->

            <ISINSymbol>
              <xsl:value-of select="COL21"/>
            </ISINSymbol>

            <SMRequest>
              <xsl:value-of select="'True'"/>
            </SMRequest>

          </PositionMaster>
        </xsl:when>
              <xsl:otherwise>
                <PositionMaster>
                <FundName>
                    <xsl:value-of select="''"/>
                </FundName>

                <PositionStartDate>
                  <xsl:value-of select="''"/>
                </PositionStartDate>

                  <Symbol>
                    <xsl:value-of select="''"/>
                  </Symbol>

                  <IDCOOptionSymbol>
                    <xsl:value-of select="''"/>
                  </IDCOOptionSymbol>

                  <SEDOL>
                     <xsl:value-of select="''"/>
                  </SEDOL>

                  <IDCOOptionSymbol>
                    <xsl:value-of select="''"/>
                  </IDCOOptionSymbol>

                <PBSymbol>
                  <xsl:value-of select="''"/>
                </PBSymbol>

                <CompanyName>
                  <xsl:value-of select="''"/>
                </CompanyName>

                <Quantity>
                  <xsl:value-of select="0"/>
                </Quantity>

                <MarkPrice>
                   <xsl:value-of select="0"/>
                </MarkPrice>

                <MarketValue>
                    <xsl:value-of select="0"/>
                </MarketValue>

                <MarketValueBase>
                   <xsl:value-of select="0"/>
                </MarketValueBase>

                <NetNotionalValue>
                    <xsl:value-of select="0"/>
                </NetNotionalValue>

                <NetNotionalValueBase>
                      <xsl:value-of select="0"/>
                </NetNotionalValueBase>

                <CUSIPSymbol>
                  <xsl:value-of select="''"/>
                </CUSIPSymbol>

                <SEDOLSymbol>
                  <xsl:value-of select="''"/>
                </SEDOLSymbol>

                <ISINSymbol>
                  <xsl:value-of select="''"/>
                </ISINSymbol>

                <SMRequest>
                  <xsl:value-of select="'true'"/>
                </SMRequest>

                </PositionMaster>
              </xsl:otherwise>
            </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

</xsl:stylesheet>
