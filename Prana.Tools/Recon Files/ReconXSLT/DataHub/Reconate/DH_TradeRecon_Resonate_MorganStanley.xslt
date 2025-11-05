<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0" exclude-result-prefixes="xs">
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
  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//Comparision">
        <xsl:variable name="Quantity">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="COL34"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="number($Quantity) and contains(COL26,'Trade')='true'">
            <PositionMaster>
              <xsl:variable name="PB_NAME">
                <xsl:value-of select="'Morgan Stanley and Co. International plc'"/>
              </xsl:variable>
              <xsl:variable name="PB_SYMBOL_NAME">
                <xsl:value-of select="COL17"/>
              </xsl:variable>
              <xsl:variable name="PRANA_SYMBOL_NAME">
                <xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
              </xsl:variable>

              <xsl:variable name="AssetType">
                <xsl:choose>
                  <xsl:when test="contains(COL15,'CALL') or contains(COL15,'PUT')">
                    <xsl:value-of select="'EquityOption'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="'Equity'"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:variable name="Symbol" select="COL19"/>
              <Symbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="$PRANA_SYMBOL_NAME"/>
                  </xsl:when>
                  <xsl:when test="$AssetType='EquityOption'">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="$AssetType='Equity'">
                    <xsl:value-of select="$Symbol"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_SYMBOL_NAME"/>
                  </xsl:otherwise>
                </xsl:choose>
              </Symbol>

              <IDCOOptionSymbol>
                <xsl:choose>
                  <xsl:when test="$PRANA_SYMBOL_NAME!=''">
                    <xsl:value-of select="''"/>
                  </xsl:when>
                  <xsl:when test="AssetType='EquityOption'">
                    <xsl:value-of select="concat(COL19,'U')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </IDCOOptionSymbol>

              <xsl:variable name="PB_COUNTER_PARTY" select="COL40"/>
              <xsl:variable name="PRANA_COUNTER_PARTY">
                <xsl:value-of select="document('../../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_COUNTER_PARTY]/@MLPBroker"/>
              </xsl:variable>
              <CounterParty>
                <xsl:choose>
                  <xsl:when test="$PRANA_COUNTER_PARTY!='' ">
                    <xsl:value-of select="$PRANA_COUNTER_PARTY"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_COUNTER_PARTY"/>
                  </xsl:otherwise>
                </xsl:choose>
              </CounterParty>
              
              <xsl:variable name="PB_FUND_NAME" select="COL3"/>
              <xsl:variable name="PRANA_FUND_NAME">
                <xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
              </xsl:variable>
              <FundName>
                <xsl:choose>
                  <xsl:when test="$PRANA_FUND_NAME!=''">
                    <xsl:value-of select="$PRANA_FUND_NAME"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="$PB_FUND_NAME"/>
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
                  <xsl:with-param name="Number" select="COL71"/>
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
              <xsl:variable name="NetNotionalValue">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL82"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValue>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValue &gt; 0">
                    <xsl:value-of select="$NetNotionalValue"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValue &lt; 0">
                    <xsl:value-of select="$NetNotionalValue * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValue>
              <xsl:variable name="NetNotionalValueBase">
                <xsl:call-template name="Translate">
                  <xsl:with-param name="Number" select="COL115"/>
                </xsl:call-template>
              </xsl:variable>
              <NetNotionalValueBase>
                <xsl:choose>
                  <xsl:when test="$NetNotionalValueBase &gt; 0">
                    <xsl:value-of select="$NetNotionalValueBase"/>
                  </xsl:when>
                  <xsl:when test="$NetNotionalValueBase &lt; 0">
                    <xsl:value-of select="$NetNotionalValueBase * (-1)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="0"/>
                  </xsl:otherwise>
                </xsl:choose>
              </NetNotionalValueBase>
              <!--<BaseCurrency>
                <xsl:value-of select="COL11"/>
              </BaseCurrency>
              <TradeDate>
                <xsl:value-of select="COL3"/>
              </TradeDate>-->
				<TradeDate>
					<xsl:value-of select="COL36"/>
				</TradeDate>
				<SettlementDate>
					<xsl:value-of select ="COL37"/>
				</SettlementDate>
              <xsl:variable name="Side" select="COL14"/>
              <Side>
                <xsl:choose>
                  <xsl:when test="$AssetType='Equity'">
                    <xsl:choose>
                      <xsl:when test="$Side='Buy Long'">
                        <xsl:value-of select="'Buy'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Sell Short'">
                        <xsl:value-of select="'Sell short'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Buy to Cover'">
                        <xsl:value-of select="'Buy to Close'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Sell Long'">
                        <xsl:value-of select="'Sell'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:when test="$AssetType='EquityOption'">
                    <xsl:choose>
                      <xsl:when test="$Side='Buy Long'">
                        <xsl:value-of select="'Buy to Open'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Sell Short'">
                        <xsl:value-of select="'Sell to Open'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Buy to Cover'">
                        <xsl:value-of select="'Buy to Close'"/>
                      </xsl:when>
                      <xsl:when test="$Side='Sell Long'">
                        <xsl:value-of select="'Sell to Close'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="''"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                </xsl:choose>
              </Side>
				
				<xsl:variable name="Commission">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL73"/>
					</xsl:call-template>
				</xsl:variable>

				<TotalCommissionandFees>

					<xsl:choose>

						<xsl:when test="$Commission &gt; 0">
							<xsl:value-of select="$Commission"/>
						</xsl:when>

						<xsl:when test="$Commission &lt; 0">
							<xsl:value-of select="$Commission * (-1)"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>

					</xsl:choose>

				</TotalCommissionandFees>

				<xsl:variable name="GrossNotionalValue">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL72"/>
					</xsl:call-template>
				</xsl:variable>

				<GrossNotionalValue>

					<xsl:choose>

						<xsl:when test="$GrossNotionalValue &gt; 0">
							<xsl:value-of select="$GrossNotionalValue"/>
						</xsl:when>

						<xsl:when test="$GrossNotionalValue &lt; 0">
							<xsl:value-of select="$GrossNotionalValue * (-1)"/>
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="0"/>
						</xsl:otherwise>

					</xsl:choose>

				</GrossNotionalValue>
				
              <PBSymbol>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </PBSymbol>
              <CompanyName>
                <xsl:value-of select="$PB_SYMBOL_NAME"/>
              </CompanyName>
              <SMRequest>
                <xsl:value-of select="'true'"/>
              </SMRequest>
            </PositionMaster>
          </xsl:when>
          <xsl:otherwise>
            <PositionMaster>
              <Symbol>
                <xsl:value-of select="''"/>
              </Symbol>
              <IDCOOptionSymbol>
                <xsl:value-of select="''"/>
              </IDCOOptionSymbol>
              <CounterParty>
                <xsl:value-of select="''"/>
              </CounterParty>
              <FundName>
                <xsl:value-of select="''"/>
              </FundName>
              <Quantity>
                <xsl:value-of select="0"/>
              </Quantity>
              <AvgPX>
                <xsl:value-of select="0"/>
              </AvgPX>
              <NetNotionalValue>
                <xsl:value-of select="0"/>
              </NetNotionalValue>
              <NetNotionalValueBase>
                <xsl:value-of select="0"/>
              </NetNotionalValueBase>
              <!--<BaseCurrency>
                <xsl:value-of select="0"/>
              </BaseCurrency>
              <TradeDate>
                <xsl:value-of select="''"/>
              </TradeDate>-->
              <Side>
                <xsl:value-of select="''"/>
              </Side>
              <PBSymbol>
                <xsl:value-of select="''"/>
              </PBSymbol>
				<GrossNotionalValue>
					<xsl:value-of select="''"/>
				</GrossNotionalValue>
              <CompanyName>
                <xsl:value-of select="''"/>
              </CompanyName>
				<TotalCommissionandFees>
					<xsl:value-of select="''"/>
				</TotalCommissionandFees>
              <SMRequest>
                <xsl:value-of select="''"/>
              </SMRequest>
            </PositionMaster>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
  <xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
  <xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>