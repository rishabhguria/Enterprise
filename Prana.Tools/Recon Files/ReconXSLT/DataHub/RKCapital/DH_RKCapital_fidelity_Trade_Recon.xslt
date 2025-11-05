<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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

	<xsl:template name="MonthCode">
		<xsl:param name="Month" />
		<xsl:param name="PutOrCall" />
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'A'" />
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'B'" />
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'C'" />
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'D'" />
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'E'" />
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'F'" />
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'G'" />
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'H'" />
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'I'" />
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'J'" />
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'K'" />
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'L'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01'">
					<xsl:value-of select="'M'" />
				</xsl:when>
				<xsl:when test="$Month='02'">
					<xsl:value-of select="'N'" />
				</xsl:when>
				<xsl:when test="$Month='03'">
					<xsl:value-of select="'O'" />
				</xsl:when>
				<xsl:when test="$Month='04'">
					<xsl:value-of select="'P'" />
				</xsl:when>
				<xsl:when test="$Month='05'">
					<xsl:value-of select="'Q'" />
				</xsl:when>
				<xsl:when test="$Month='06'">
					<xsl:value-of select="'R'" />
				</xsl:when>
				<xsl:when test="$Month='07'">
					<xsl:value-of select="'S'" />
				</xsl:when>
				<xsl:when test="$Month='08'">
					<xsl:value-of select="'T'" />
				</xsl:when>
				<xsl:when test="$Month='09'">
					<xsl:value-of select="'U'" />
				</xsl:when>
				<xsl:when test="$Month='10'">
					<xsl:value-of select="'V'" />
				</xsl:when>
				<xsl:when test="$Month='11'">
					<xsl:value-of select="'W'" />
				</xsl:when>
				<xsl:when test="$Month='12'">
					<xsl:value-of select="'X'" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(normalize-space(COL3),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring(substring-after(normalize-space(COL3),' '),5,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring(substring-after(normalize-space(COL3),' '),3,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring(substring-after(normalize-space(COL3),' '),1,2)"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after(normalize-space(COL3),' '),7,1)"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring(substring-after(normalize-space(COL3),' '),8) div 1000,'#.00')"/>
		</xsl:variable>
		<xsl:variable name="MonthCodVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="$ExpiryMonth"/>
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
		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ', $ExpiryYear,$MonthCodVar,$StrikePrice,'D',$Day)"/>
	</xsl:template>
  <xsl:template match="/">
    <DocumentElement>
		
		
	<xsl:for-each select ="//Comparision">
		  
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL6"/>
          </xsl:call-template>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="number($Quantity)">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="''"/>
              </xsl:variable>

              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL3"/>
              </xsl:variable>

              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name="varSymbol">
                <xsl:value-of select="COL3"/>
              </xsl:variable>

				<xsl:variable name="varAsset">
					<xsl:choose>
						<xsl:when test="string-length(COL3) &gt; 15">
							<xsl:value-of select="'EquityOption'" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'Equity'" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
					<xsl:when test="$varAsset='EquityOption'">
						<xsl:call-template name="Option">
							<xsl:with-param name="Symbol" select="normalize-space(COL3)" />
						</xsl:call-template>
					</xsl:when>
                  <xsl:when test="$varSymbol!=''">
                    <xsl:value-of select="$varSymbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>


              <xsl:variable name="PB_FUND_NAME" select="COL2"/>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select ="document('../../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test ="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select ="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="$PB_FUND_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </FundName>

              <Quantity>
                <xsl:choose>
                  <xsl:when test="$Quantity &gt; 0">
                    <xsl:value-of select="$Quantity"/>
                  </xsl:when>

                  <xsl:when test="$Quantity &lt; 0">
                    <xsl:value-of select="$Quantity * -1"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Quantity>

               <xsl:variable name="AvgPrice">
              <xsl:call-template name="Translate">
                <xsl:with-param name="Number" select="COL7"/>
              </xsl:call-template>
            </xsl:variable>
            <AvgPX>
              <xsl:choose>
                <xsl:when test="$AvgPrice &gt; 0">
                  <xsl:value-of select="$AvgPrice"/>
                </xsl:when>
                <xsl:when test="$AvgPrice &lt; 0">
                  <xsl:value-of select="$AvgPrice * (-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </AvgPX>

				<xsl:variable name="varCommission">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>
				<Commission>
					<xsl:choose>
						<xsl:when test="$varCommission &gt; 0">
							<xsl:value-of select="$varCommission"/>
						</xsl:when>
						<xsl:when test="$varCommission &lt; 0">
							<xsl:value-of select="$varCommission * (-1)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</Commission>


				<!--<xsl:variable name="varTradeDay">
                   <xsl:value-of select="substring-before(substring-after(COL5,'/'),'/')"/>
				</xsl:variable>
				<xsl:variable name="varTradeMonth">
					<xsl:value-of select="substring-before(COL5,'/')"/>
				</xsl:variable>
				<xsl:variable name="varTradeYear">
                   <xsl:value-of select="substring-after(substring-after(COL5,'/'),'/')"/>
				</xsl:variable>
				<xsl:variable name="varTradeDate">
					<xsl:value-of select="concat($varTradeMonth,'/',$varTradeDay,'/',$varTradeYear)"/>
				</xsl:variable>-->

				<TradeDate>
					<xsl:value-of select="COL1"/>
				</TradeDate>
				


              <xsl:variable name="varSide">
                <xsl:value-of select="COL4"/>
              </xsl:variable>
              <Side>
                
                <xsl:choose>
                  <xsl:when test="$varAsset='EquityOption'">
                    <xsl:choose>
                      <xsl:when test="$varSide='BUY' ">
                        <xsl:value-of select="'Buy to Close'"/>
                      </xsl:when>
                      <xsl:when test="$varSide='SELL' ">
                        <xsl:value-of select="'Sell to Close'"/>
                      </xsl:when>

                     
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                     <xsl:when test="$varSide='BUY' ">
                        <xsl:value-of select="'Buy'"/>
                      </xsl:when>
                     <xsl:when test="$varSide='SELL' ">
                        <xsl:value-of select="'Sell'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </Side>

              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>

				<!--<xsl:variable name="PB_BROKER_NAME">
					<xsl:value-of select="normalize-space(COL5)"/>
				</xsl:variable>

				<xsl:variable name="PRANA_BROKER_ID">
					<xsl:value-of select="document('../../ReconMappingXML/ReconMappingXml/CounterPartyMapping.xml')/CounterPartyMapping/PB[@Name=$PB_NAME]/CounterPartyData[@MappedBrokerCode=$PB_BROKER_NAME]/@BrokerCode"/>
				</xsl:variable>

				<CounterPartyID>
					<xsl:choose>
						<xsl:when test="number($PRANA_BROKER_ID!='')">
							<xsl:value-of select="$PRANA_BROKER_ID"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>
					</xsl:choose>
				</CounterPartyID>-->

            </PositionMaster>
          </xsl:when>

          <xsl:otherwise>
            <PositionMaster>

              <Symbol>
                <xsl:value-of select ="''"/>
              </Symbol>

              <FundName>
                <xsl:value-of select ="''"/>
              </FundName>

              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>

				<Commission>
					  <xsl:value-of select="0"/>				
				</Commission>
				
              <TradeDate>
                <xsl:value-of select ="''"/>
              </TradeDate>
				
				
              <Side>
                <xsl:value-of select="''"/>
              </Side>

              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>
				
			   <AvgPX>
               <xsl:value-of select="0"/>
			 </AvgPX>


            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>

  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


