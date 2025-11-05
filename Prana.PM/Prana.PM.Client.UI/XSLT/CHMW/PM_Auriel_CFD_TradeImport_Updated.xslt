<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month = 'Jan'">
				<xsl:value-of select="'01'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Feb'">
				<xsl:value-of select="'02'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Mar'">
				<xsl:value-of select="'03'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Apr'">
				<xsl:value-of select="'04'"/>
			</xsl:when>
			<xsl:when test="$Month = 'May'">
				<xsl:value-of select="'05'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jun'">
				<xsl:value-of select="'06'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Jul'">
				<xsl:value-of select="'07'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Aug'">
				<xsl:value-of select="'08'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Sep'">
				<xsl:value-of select="'09'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Oct'">
				<xsl:value-of select="'10'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Nov'">
				<xsl:value-of select="'11'"/>
			</xsl:when>
			<xsl:when test="$Month = 'Dec'">
				<xsl:value-of select="'12'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL10) and COL4!='CA'">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Credit Suisse Securities (Europe) Ltd'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL11"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--<xsl:variable name = "PB_SUFFIX_CODE" >
							<xsl:value-of select ="substring-after(COL12,'.')"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_CODE]/@PranaSuffixCode"/>
						</xsl:variable>-->


						<Symbol>


							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<!--<xsl:when test="COL13!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>-->
								<xsl:when test="COL15!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL14!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>

						<CUSIP>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<!--<xsl:when test="COL13!='*'">
                  <xsl:value-of select="COL13"/>
                </xsl:when>-->
								<xsl:when test="COL15!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL14!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</CUSIP>


						<SEDOL>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>

								<!--<xsl:when test="COL13!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>-->
								<xsl:when test="COL14!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL15!='*'">
									<xsl:value-of select="COL15"/>
								</xsl:when>



								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</SEDOL>


						<ISIN>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<!--<xsl:when test="COL13!='*'">
                  <xsl:value-of select="''"/>
                </xsl:when>-->

								<xsl:when test="COL14!='*'">
									<xsl:value-of select="COL14"/>
								</xsl:when>
								<xsl:when test="COL15!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</ISIN>

						<Symbology>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="'Symbol'"/>
								</xsl:when>

								<!--<xsl:when test="COL13!='*'">
                  <xsl:value-of select="'CUSIP'"/>
                </xsl:when>-->
								<xsl:when test="COL14!='*'">
									<xsl:value-of select="'ISIN'"/>
								</xsl:when>
								<xsl:when test="COL15!='*'">
									<xsl:value-of select="'Sedol'"/>
								</xsl:when>





								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbology>

						<SecurityRequestDescription>
							<xsl:value-of select="'CFD'"/>
						</SecurityRequestDescription>


						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<xsl:variable name="Month">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(COL6,'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Month1">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(COL7,'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<PositionStartDate>
							<xsl:value-of select="concat($Month,'/',substring-before(substring-after(COL6,'-'),'-'),'/',substring-after(substring-after(COL6,'-'),'-'))"/>

						</PositionStartDate>


						<PositionSettlementDate>
							<xsl:value-of select="concat($Month1,'/',substring-before(substring-after(COL7,'-'),'-'),'/',substring-after(substring-after(COL7,'-'),'-'))"/>
						</PositionSettlementDate>


						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="number(COL10)"/>
						</xsl:variable>

						<NetPosition>

							<xsl:choose>

								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetPosition>

						<SideTagValue>

							<xsl:choose>
								<xsl:when test="COL4='TR' and COL8='SELL' and COL9='*'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL4='C' and COL8='SELL' and COL9='*'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL4='TR' and COL8='BUY' and COL9='*'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL4='C' and COL8='BUY' and COL9='*'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="COL4='CA' and COL8='BUY' and COL9='*'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="COL4='CA' and COL8='SELL' and COL9='*'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test ="COL8='SELL' and COL9='OPEN'">
											<xsl:value-of select ="'5'"/>
										</xsl:when>

										<xsl:when test ="COL8='SELL' and COL9='CLOSE'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>


										<xsl:when test ="COL8='BUY' and COL9='CLOSE'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>

										<xsl:when test ="COL8='BUY' and COL9='OPEN'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:otherwise>
							</xsl:choose>

						</SideTagValue>

						<!--<xsl:variable name ="varCostBasis">
							<xsl:value-of select ="COL44 div COL10"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="format-number($varCostBasis,'0.####')*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="format-number($varCostBasis,'0.####')"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>-->

						<xsl:variable name ="varCostBasis">
							<xsl:choose>
								<xsl:when test="COL17 &gt; 0.0001">
									<xsl:value-of select="COL17"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<xsl:variable name ="varExchangeDescription">
							<xsl:value-of select="COL28"/>
						</xsl:variable>

						<xsl:variable name ="varExchangeCode">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXML/ExchangeCodeMapping.xml')/ExchangeCodeMapping/PB[@Name=$PB_NAME]/ExchangeCodeData[@PBExchangeDescription=$varExchangeDescription]/@ExchangeCode"/>
						</xsl:variable>

						<ExchangeCode>
							<xsl:choose>

								<xsl:when test ="$varExchangeCode!=''">
									<xsl:value-of select ="$varExchangeCode"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</ExchangeCode>

						<IsPreferPrimaryExchange>
							<xsl:choose>

								<xsl:when test ="$varExchangeCode!=''">
									<xsl:value-of select ="'false'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="'true'"/>
								</xsl:otherwise>

							</xsl:choose>
						</IsPreferPrimaryExchange>




						<!--<xsl:variable name ="varCommision">
							<xsl:value-of select ="number(COL44) - number(COL47)"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>

								<xsl:when test ="$varCommision &lt;0">
									<xsl:value-of select ="$varCommision*-1"/>
								</xsl:when>

								<xsl:when test ="$varCommision &gt;0">
									<xsl:value-of select ="$varCommision"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>-->



						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>



						<IsSwapped>
							<!--<xsl:choose>
								<xsl:when test ="normalize-space(COL36)= 'YXQOU0'">-->
							<xsl:value-of select ="1"/>
							<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>-->

						</IsSwapped>

						<SwapDescription>
							<!--<xsl:choose>
								<xsl:when test ="normalize-space(COL36)= 'YXQOU0'">-->
							<xsl:value-of select ="'CFD'"/>
							<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'a'"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</SwapDescription>

						<DayCount>
							<!--<xsl:choose>
								<xsl:when test ="normalize-space(COL36)= 'YXQOU0'">-->
							<xsl:value-of select ="365"/>
							<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="1"/>
								</xsl:otherwise>
							</xsl:choose>-->

						</DayCount>

						<ResetFrequency>
							<!--<xsl:choose>
								<xsl:when test ="normalize-space(COL36)= 'YXQOU0'">
									<xsl:value-of select ="'Monthly'"/>
								</xsl:when>
								<xsl:otherwise>-->
							<xsl:value-of select ="'Monthly'"/>
							<!--</xsl:otherwise>
							</xsl:choose>-->
						</ResetFrequency>

						<OrigTransDate>
							<!--<xsl:choose>
								<xsl:when test ="normalize-space(COL36)= 'YXQOU0'">-->
							<xsl:value-of select ="COL6"/>
							<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'01/01/1800'"/>
								</xsl:otherwise>
							</xsl:choose>-->

						</OrigTransDate>

						<xsl:variable name="varPreviousMonth">
							<xsl:value-of select="substring-before(COL6,'-')"/>
						</xsl:variable>



						<xsl:variable name ="varPrevMonth">
							<xsl:choose>
								<xsl:when test="$varPreviousMonth='Jan'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Feb'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Mar'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Apr'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='May'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Jun'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Jul'">
									<xsl:value-of select="6"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Aug'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Sep'">
									<xsl:value-of select="8"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Oct'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Nov'">
									<xsl:value-of select="10"/>
								</xsl:when>
								<xsl:when test="$varPreviousMonth='Dec'">
									<xsl:value-of select="11"/>
								</xsl:when>
							</xsl:choose>
							<!--<xsl:choose>
								<xsl:when test ="substring-before(COL6,'/') = 1">
									<xsl:value-of select ="12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="number(substring-before(COL6,'/'))-1"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</xsl:variable>

						<xsl:variable name="varYearNo">
							<xsl:value-of select="substring-after(substring-after(COL6,'-'),'-')"/>
						</xsl:variable>

						<xsl:variable name ="varYear">
							<xsl:choose>
								<xsl:when test="$varPreviousMonth='Jan'">
									<xsl:value-of select="($varYearNo)-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varYearNo"/>
								</xsl:otherwise>
							</xsl:choose>
							<!--<xsl:choose>
								<xsl:when test ="substring-before(COL6,'/') = 1">
									<xsl:value-of select ="number(substring-after(substring-after(COL6,'/'),'/')) -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="number(substring-after(substring-after(COL6,'/'),'/'))"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</xsl:variable>


						<FirstResetDate>
							<!--<xsl:choose>
								<xsl:when test ="normalize-space(COL36)= 'YXQOU0'">-->
							<xsl:value-of select ="concat($varPrevMonth,'/28/',$varYear)"/>
							<!--</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="'01/01/1800'"/>
								</xsl:otherwise>
							</xsl:choose>-->
						</FirstResetDate>

						<!--<xsl:variable name="varFXRate">
							<xsl:value-of select="number(COL45 div COL44)"/>
						</xsl:variable>-->

						<!--<FXRate>
							<xsl:choose>

								<xsl:when test ="$varFXRate &lt;0">
									<xsl:value-of select ="$varFXRate*-1"/>
								</xsl:when>

								<xsl:when test ="$varFXRate &gt;0">
									<xsl:value-of select ="$varFXRate"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</FXRate>-->

						<!--<FXConversionMethodOperator>
							<xsl:value-of select ="'M'"/>
						</FXConversionMethodOperator>-->

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
