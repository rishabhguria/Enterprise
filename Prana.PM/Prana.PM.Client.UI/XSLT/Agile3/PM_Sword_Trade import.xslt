<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>




	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="(COL7)"/>
					</xsl:call-template>
				</xsl:variable>

				<!--<xsl:variable name="Position">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="(COL7 * 3)"/>
          </xsl:call-template>
        </xsl:variable>-->

				<xsl:if test="number($Position) and COL5!='38142B609' and COL4!='EXW' ">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'BTIG'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL5"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name ="Asset">
							<xsl:choose>
								<xsl:when test="string-length(COL5) = 21">
									<xsl:value-of select="'Option'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Symbol" select="COL5"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
									<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test ="string-length(COL5) = 21">
									<xsl:value-of select ="''"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="COL5"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<IDCOOptionSymbol>
							<xsl:choose>
								<xsl:when test ="string-length(COL5) = 21">
									<xsl:value-of select="concat(COL5, 'U')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</IDCOOptionSymbol>


						<xsl:variable name="PB_FUND_NAME" select="COL26"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<NetPosition>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetPosition>

						<xsl:variable name="Side" select="COL4"/>


						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Side='BUY' and $Asset='Option'">
									<xsl:value-of select="'A'"/>
								</xsl:when>
								<xsl:when test="$Side='BTC' and $Asset='Option'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$Side='SEL' and $Asset='Option'">
									<xsl:value-of select="'D'"/>
								</xsl:when>

								<xsl:when test="$Side='SSL' and $Asset='Option'">
									<xsl:value-of select="'C'"/>
								</xsl:when>

								<xsl:when test="$Side='BUY'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
							

								<xsl:when test="$Side='SEL' and contains(COL5, '_C')">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:when test="$Side='SEL'">
									<xsl:value-of select="'2'"/>
								</xsl:when>

								<xsl:when test="$Side='BTC'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$Side='SSL'">
									<xsl:value-of select="'5'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>


						<xsl:variable name="COL8">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL8"/>
							</xsl:call-template>
						</xsl:variable>




						<!--<xsl:variable name="CostBasis">
							<xsl:choose>
								<xsl:when test="COL2 = 'Option'">
									<xsl:value-of select="$COL12 div ($Position * 100)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$COL12 div $Position"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<CostBasis>
							<xsl:choose>

								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>
								</xsl:when>

								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>-->
						<xsl:variable name="Cost">
							<xsl:value-of select="COL8"/>
						</xsl:variable>



						<xsl:choose>
							<xsl:when test ="boolean(number($Cost))">
								<CostBasis>
									<xsl:value-of select="$Cost"/>
								</CostBasis>
							</xsl:when>
							<xsl:otherwise>
								<CostBasis>
									<xsl:value-of select="0"/>
								</CostBasis>
							</xsl:otherwise>
						</xsl:choose>

						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>
						
						
						
						
						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL9"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>

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

						</Commission>

						<xsl:variable name="Fees">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL10"/>
							</xsl:call-template>
						</xsl:variable>

						<Fees>

							<xsl:choose>

								<xsl:when test="$Fees &gt; 0">
									<xsl:value-of select="$Fees"/>
								</xsl:when>

								<xsl:when test="$Fees &lt; 0">
									<xsl:value-of select="$Fees * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</Fees>

						<xsl:variable name="StampDuty">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL19"/>
							</xsl:call-template>
						</xsl:variable>

						<StampDuty>

							<xsl:choose>

								<xsl:when test="$StampDuty &gt; 0">
									<xsl:value-of select="$StampDuty"/>
								</xsl:when>

								<xsl:when test="$StampDuty &lt; 0">
									<xsl:value-of select="$StampDuty * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</StampDuty>






						<!--<xsl:variable name="FXRate" select="COL25"/>

						<FXRate>
							<xsl:choose>

								<xsl:when test ="number($FXRate) ">
									<xsl:value-of select =" COL23 div COL22 "/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</FXRate>-->


						<!--<xsl:variable name="Year1" select="substring($Date,1,4)"/>
            <xsl:variable name="Month" select="substring($Date,5,2)"/>
            <xsl:variable name="Day" select="substring($Date,7,2)"/>-->
						<xsl:variable name="PB_CountnerParty" select="COL16"/>
						<xsl:variable name="PRANA_CounterPartyID">
							<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name= 'BTIG']/BrokerData[@PranaBroker=$PB_CountnerParty]/@PranaBrokerCode"/>
						</xsl:variable>

						<CounterPartyID>
							<xsl:choose>
								<xsl:when test="number($PRANA_CounterPartyID)">
									<xsl:value-of select="$PRANA_CounterPartyID"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CounterPartyID>
						

						<PositionStartDate>
           
              <xsl:value-of select="COL2"/>
            </PositionStartDate>

						<!--<OriginalPurchaseDate>
							<xsl:value-of select="COL3"/>
						</OriginalPurchaseDate>-->

						
						

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>