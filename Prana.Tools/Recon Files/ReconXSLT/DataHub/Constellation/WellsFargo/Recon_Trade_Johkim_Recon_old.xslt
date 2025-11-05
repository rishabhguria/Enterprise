<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">
				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL11"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:choose>
					<xsl:when test="number($Quantity) and contains(COL3,'Cash')!='true'">
						<PositionMaster>
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="'WellsFargo'"/>
							</xsl:variable>

							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="normalize-space(COL10)"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>

							<xsl:variable name="PB_SUFFIX_NAME">
								<xsl:value-of select="substring-after(COL9,' ')"/>
							</xsl:variable>

							<xsl:variable name="PRANA_SUFFIX_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@PranaSuffixCode"/>
							</xsl:variable>


							<!--<xsl:variable name="Symbol" select="substring-before(COL9,' ')"/>-->
							<xsl:variable name="Symbol">
								<xsl:choose>
									<xsl:when test="contains(COL9,' ')">
										<xsl:value-of select="substring-before(COL9,' ')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="COL9"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>

									<xsl:when test ="$Symbol!=''">
										<xsl:value-of select ="concat($Symbol,$PRANA_SUFFIX_NAME)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>

							</Symbol>



							<xsl:variable name="PB_FUND_NAME" select="'Johkim'"/>
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


							<xsl:variable name="TradeDate" select="COL4"/>

							<TradeDate>
								<xsl:value-of select="$TradeDate"/>
							</TradeDate>


							<Quantity>
								<xsl:choose>
									<xsl:when test="$Quantity &gt; 0">
										<xsl:value-of select="$Quantity"/>
									</xsl:when>
									<xsl:when test="$Quantity &lt; 0">
										<xsl:value-of select="$Quantity * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>


							<!--<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL12"/>
							</xsl:call-template>
						</xsl:variable>


						<MarkPrice>
							<xsl:choose>
								<xsl:when test="$MarkPrice &gt; 0">
									<xsl:value-of select="$MarkPrice"/>

								</xsl:when>
								<xsl:when test="$MarkPrice &lt; 0">
									<xsl:value-of select="$MarkPrice * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>-->
							<xsl:variable name="varAvgPX">
								<xsl:value-of select="COL12"/>
							</xsl:variable>
							<AvgPX>
								<xsl:choose>
									<xsl:when test="number($varAvgPX)">
										<xsl:value-of select="$varAvgPX"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</AvgPX>


							<xsl:variable name="Side" select="COL8"/>

							<Side>

								<xsl:choose>
									<xsl:when test="$Side='BY'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>

									<xsl:when test="$Side='SL'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>

									<xsl:when test="$Side='SS'">
										<xsl:value-of select="'Sell Short'"/>
									</xsl:when>


								</xsl:choose>

							</Side>




							<xsl:variable name="Commission">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL17"/>
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

							<xsl:variable name="varStampDuty">
								<xsl:value-of select="COL14"/>
							</xsl:variable>
							<varStampDuty>
								<xsl:choose>
									<xsl:when test="$varStampDuty &gt; 0">
										<xsl:value-of select="$varStampDuty"/>
									</xsl:when>
									<xsl:when test="$varStampDuty &lt; 0">
										<xsl:value-of select="$varStampDuty * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</varStampDuty>

							<xsl:variable name="SecFee">
								<xsl:value-of select="COL15"/>
							</xsl:variable>
							<SecFee>
								<xsl:choose>
									<xsl:when test="$SecFee &gt; 0">
										<xsl:value-of select="$SecFee"/>
									</xsl:when>
									<xsl:when test="$SecFee &lt; 0">
										<xsl:value-of select="$SecFee * (-1)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecFee>

							<xsl:variable name="TotalCommissionandFees">
								<xsl:value-of select="number($Commission) + number($varStampDuty) + number($SecFee) "/>
							</xsl:variable>
							<TotalCommissionandFees>
								<xsl:choose>
									<xsl:when test="$TotalCommissionandFees &gt; 0">
										<xsl:value-of select="format-number($TotalCommissionandFees, '#.##')"/>
									</xsl:when>
									<xsl:when test="$TotalCommissionandFees &lt; 0">
										<xsl:value-of select="format-number($TotalCommissionandFees*-1,'#.##')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</TotalCommissionandFees>




							<xsl:variable name="NetNotionalValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL19"/>
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

							<xsl:variable name="varNetNotionalValueBase">
								<xsl:choose>
									<xsl:when test="contains(COL23,'_')='JPY'">
										<xsl:value-of select="(COL19 div COL25)"/>
									</xsl:when>

									<xsl:otherwise>
										<xsl:value-of select="(COL19 * COL25)"/>
									</xsl:otherwise>
								</xsl:choose>

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


							<xsl:variable name="GrossNotionalValue">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="COL13"/>
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

							<!--<CurrencySymbol>
							<xsl:value-of select="COL23"/>
						</CurrencySymbol>-->

							<Companyname>
								<xsl:value-of select="$PB_SYMBOL_NAME"/>
							</Companyname>


						</PositionMaster>

					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>
							

							<Symbol>
								<xsl:value-of select="''"/>

							</Symbol>


							<FundName>
								<xsl:value-of select="''"/>
							</FundName>


						
							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>


							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>


							
							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>



							<Side>

								<xsl:value-of select="''"/>
							</Side>

							<Commission>
								<xsl:value-of select="0"/>
							</Commission>

							
							<varStampDuty>
								<xsl:value-of select="0"/>
							</varStampDuty>

							<SecFee>
								<xsl:value-of select="0"/>
							</SecFee>

							
							<TotalCommissionandFees>
								<xsl:value-of select="0"/>
							</TotalCommissionandFees>

							<NetNotionalValue>
								<xsl:value-of select="0"/>
							</NetNotionalValue>

						
							<NetNotionalValueBase>

								<xsl:value-of select="0"/>
							</NetNotionalValueBase>


						
							<GrossNotionalValue>
								<xsl:value-of select="0"/>
							</GrossNotionalValue>

							

							<Companyname>
								<xsl:value-of select="''"/>
							</Companyname>


						</PositionMaster>

					</xsl:otherwise>
				</xsl:choose>


				
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>