<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

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

	</xsl:template>

	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>
		<xsl:choose>
			<xsl:when test="$varMonth=1">
				<xsl:value-of select="'F'"/>
			</xsl:when>
			<xsl:when test="$varMonth=2">
				<xsl:value-of select="'G'"/>
			</xsl:when>
			<xsl:when test="$varMonth=3">
				<xsl:value-of select="'H'"/>
			</xsl:when>
			<xsl:when test="$varMonth=4">
				<xsl:value-of select="'J'"/>
			</xsl:when>
			<xsl:when test="$varMonth=5">
				<xsl:value-of select="'K'"/>
			</xsl:when>
			<xsl:when test="$varMonth=6">
				<xsl:value-of select="'M'"/>
			</xsl:when>
			<xsl:when test="$varMonth=7">
				<xsl:value-of select="'N'"/>
			</xsl:when>
			<xsl:when test="$varMonth=8">
				<xsl:value-of select="'Q'"/>
			</xsl:when>
			<xsl:when test="$varMonth=9">
				<xsl:value-of select="'U'"/>
			</xsl:when>
			<xsl:when test="$varMonth=10">
				<xsl:value-of select="'V'"/>
			</xsl:when>
			<xsl:when test="$varMonth=11">
				<xsl:value-of select="'X'"/>
			</xsl:when>
			<xsl:when test="$varMonth=12">
				<xsl:value-of select="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="MonthsCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
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
				<xsl:when test="$Month=1 ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2 ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3 ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4 ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5 ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6 ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9 ">
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
		<xsl:if test="(contains(substring(substring-before(COL19,' '),1,1),'P') or contains(substring(substring-before(COL19,' '),1,1),'C')) and (COL4='48604' or COL4='48601')">
			<xsl:variable name="PB_UnderlyingSymbol_Name">
				<xsl:value-of select="substring-before(substring-after(normalize-space(COL19),' '),' ')"/>
			</xsl:variable>

			<xsl:variable name="PRANA_WEAK_SYMBOL">
				<xsl:value-of select="document('../ReconMappingXML/WeakSymbolMapping.xml')/SymbolMapping/PB[@Name='NewEdge']/SymbolData[@PBCompanyName=$PB_UnderlyingSymbol_Name]/@PranaSymbol"/>
			</xsl:variable>

			<xsl:variable name="UnderlyingSymbol">
				<xsl:choose>
					<xsl:when test="$PRANA_WEAK_SYMBOL=''">
						<xsl:value-of select="$PB_UnderlyingSymbol_Name"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$PRANA_WEAK_SYMBOL"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(COL11,7,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(COL11,5,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(COL11,3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(COL19,' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="COL10"/>
			</xsl:variable>


			<xsl:variable name="MonthCodeVar">
				<xsl:call-template name="MonthsCode">
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



			<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ','INDEX',' ',$ExpiryMonth,'/',$ExpiryDay,'/',$ExpiryYear,' ',$PutORCall,$StrikePrice,'-EEO')"/>

		</xsl:if>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL18"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and contains(COL1,'T')='true' and COL4!='48600' and COL4!='48601'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'NewEdge'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL19"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_ROOT_NAME">
							<xsl:value-of select="normalize-space(COL24)"/>
						</xsl:variable>

					

						<xsl:variable name="PB_EXCHANGE_NAME">
							<xsl:value-of select="normalize-space(COL23)"/>
						</xsl:variable>

					
						
						<xsl:variable name ="PRANA_ROOT_NAME">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode = $PB_ROOT_NAME and @ExchangeName = $PB_EXCHANGE_NAME]/@UnderlyingCode"/>
						</xsl:variable>
						<xsl:variable name ="FUTURE_EXCHANGE_CODE_WITH_EXCH">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME and @ExchangeName = $PB_EXCHANGE_NAME]/@ExchangeCode"/>
						</xsl:variable>
						

						<xsl:variable name ="FUTURE_EXCHANGE_CODE">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExchangeCode"/>
						</xsl:variable>

						<xsl:variable  name="FUTURE_FLAG">
							<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_ROOT_NAME]/@ExpFlag"/>
						</xsl:variable>

						<xsl:variable name="MonthCode">
							<xsl:call-template name="MonthCode">
								<xsl:with-param name="varMonth" select="number(substring(COL7,5,2))"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Year" select="substring(COL7,4,1)"/>

						<xsl:variable name="MonthYearCode">
							<xsl:choose>
								<xsl:when test="$FUTURE_FLAG!=''">
									<xsl:value-of select="concat($Year,$MonthCode)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat($MonthCode,$Year)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="Underlying">
							<xsl:choose>
								<xsl:when test="$PRANA_ROOT_NAME!=''">
									<xsl:value-of select="$PRANA_ROOT_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_ROOT_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>


							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>


								<xsl:when test="(contains(substring(substring-before(COL19,' '),1,1),'P') or contains(substring(substring-before(COL19,' '),1,1),'C')) and (COL4='48604' or COL4='48601')">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="COL19"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>

								<xsl:when test="normalize-space(COL8)='F'">
									<xsl:choose>
										<xsl:when test="$FUTURE_EXCHANGE_CODE_WITH_EXCH != ''">
											<xsl:value-of select="normalize-space(concat($Underlying,' ',$MonthYearCode,$FUTURE_EXCHANGE_CODE_WITH_EXCH))"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="normalize-space(concat($Underlying,' ',$MonthYearCode,$FUTURE_EXCHANGE_CODE))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>

						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="'SG'"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>
								<xsl:when test="COL4='48603'">
									<xsl:value-of select="'KSSGFUT'"/>
								</xsl:when>
								<xsl:when test="COL4='48604'">
									<xsl:value-of select="'KSSGOPT'"/>
								</xsl:when>
                                                                  <xsl:when test="COL4='48605'">
									<xsl:value-of select="'KFSGFUT'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>

										<xsl:when test ="$PRANA_FUND_NAME!=''">
											<xsl:value-of select ="$PRANA_FUND_NAME"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="$PB_FUND_NAME"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<Multiplier>
							<xsl:value-of select="COL92"/>
						</Multiplier>


						<Quantity>
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
						</Quantity>

						<xsl:variable name="AvgPX">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL27"/>
							</xsl:call-template>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPX &gt; 0">
									<xsl:value-of select="$AvgPX"/>

								</xsl:when>
								<xsl:when test="$AvgPX &lt; 0">
									<xsl:value-of select="$AvgPX * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</AvgPX>

						<xsl:variable name="Side" select="normalize-space(COL13)"/>

						<Side>
							<xsl:choose>

								<xsl:when test="COL4='48604'">
									<xsl:choose>
										<xsl:when test="$Side='1'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>

										<xsl:when test="$Side='2' and COL32='O'">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>

										<xsl:when test="$Side='2' and COL32='C'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>

								<xsl:when test="COL4='48600' or COL4='48603' or COL4='48605'">
									<xsl:choose>
										<xsl:when test="$Side='1' and (COL9='P' or COL9='C')">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>

										<xsl:when test="$Side='2' and (COL9='P' or COL9='C')">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test="$Side='1'">
													<xsl:value-of select="'Buy'"/>
												</xsl:when>

												<xsl:when test="$Side='2'">
													<xsl:value-of select="'Sell'"/>
												</xsl:when>

												<xsl:otherwise>
													<xsl:value-of select="''"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="$Side='1'">
												<xsl:value-of select="'Buy'"/>
											</xsl:when>

											<xsl:when test="$Side='2'">
												<xsl:value-of select="'Sell'"/>
											</xsl:when>

											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								

							</xsl:choose>
						</Side>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="TradeDateDay" select="substring(normalize-space(COL12),7,2)"/>
						<xsl:variable name="TradeDateMonth" select="substring(normalize-space(COL12),5,2)"/>
						<xsl:variable name="TradeDateYear" select="substring(normalize-space(COL12),1,4)"/>

						<TradeDate>
							<xsl:value-of select="concat($TradeDateMonth,'/',$TradeDateDay,'/',$TradeDateYear)"/>
						</TradeDate>

						<xsl:variable name="varDays" select="substring(normalize-space(COL21),7,2)"/>

						<xsl:variable name="varMonths" select="substring(normalize-space(COL21),5,2)"/>

						<xsl:variable name="varYear" select="substring(normalize-space(COL21),1,4)"/>

						<SettlementDate>
							<xsl:value-of select="concat($varMonths,'/',$varDays,'/',$varYear)" />
						</SettlementDate>

						<CurrencySymbol>
							<xsl:value-of select="COL29"/>
						</CurrencySymbol>

						<SettlCurrency>
							<xsl:value-of select="COL29"/>
						</SettlCurrency>

						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL37"/>
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

						<xsl:variable name="SecFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL40"/>
							</xsl:call-template>
						</xsl:variable>

						<StampDuty>
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
						</StampDuty>

						<xsl:variable name="COL39">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL39)"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL40">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL40)"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL38">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL38)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="OtherBrokerFees" select="number($COL38 + $COL40 + $COL39 + COL104)"/>

						<OtherBrokerFees>
							<xsl:choose>
								<xsl:when test="$OtherBrokerFees &gt; 0">
									<xsl:value-of select="$OtherBrokerFees"/>

								</xsl:when>
								<xsl:when test="$OtherBrokerFees &lt; 0">
									<xsl:value-of select="$OtherBrokerFees * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</OtherBrokerFees>

						<xsl:variable name="COL44">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL44)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="COL101">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL101)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="TransLevy" select="number($COL44 + $COL101)"/>

						<TransactionLevy>
							<xsl:choose>
								<xsl:when test="$TransLevy &gt; 0">
									<xsl:value-of select="$TransLevy"/>

								</xsl:when>
								<xsl:when test="$TransLevy &lt; 0">
									<xsl:value-of select="$TransLevy * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</TransactionLevy>

						<xsl:variable name="COL41">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL41)"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL100">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL100)"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL102">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL102)"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL103">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL103)"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="COL104">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL104)"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="OtherFee">
							<xsl:value-of select="number($COL41 + $COL100 + $COL102 + $COL103 + $COL104)"/>
						</xsl:variable>

						<MiscFees>
							<xsl:choose>
								<xsl:when test="$OtherFee &gt; 0">
									<xsl:value-of select="$OtherFee"/>

								</xsl:when>
								<xsl:when test="$OtherFee &lt; 0">
									<xsl:value-of select="$OtherFee * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MiscFees>

						<xsl:variable name="ClearingFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL44"/>
							</xsl:call-template>
						</xsl:variable>

						<ClearingFee>
							<xsl:choose>
								<xsl:when test="$ClearingFee &gt; 0">
									<xsl:value-of select="$ClearingFee"/>

								</xsl:when>
								<xsl:when test="$ClearingFee &lt; 0">
									<xsl:value-of select="$ClearingFee * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</ClearingFee>

						<xsl:variable name="AUECFee">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL39"/>
							</xsl:call-template>
						</xsl:variable>

						<ClearingBrokerFee>
							<xsl:choose>
								<xsl:when test="$AUECFee &gt; 0">
									<xsl:value-of select="$AUECFee"/>

								</xsl:when>
								<xsl:when test="$AUECFee &lt; 0">
									<xsl:value-of select="$AUECFee * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</ClearingBrokerFee>

						

						<xsl:variable name="NetNotional">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL46"/>
							</xsl:call-template>
						</xsl:variable>

						<NetNotionalValue>
							<xsl:choose>
								<xsl:when test="$Side='1'">
							<xsl:choose>
								<xsl:when test="$NetNotional &gt; 0">
									<xsl:value-of select="$NetNotional"/>
								</xsl:when>
								<xsl:when test="$NetNotional &lt; 0">
									<xsl:value-of select="$NetNotional * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
							</xsl:when>
								<xsl:when test="$Side='2'">
									<xsl:choose>
										<xsl:when test="$NetNotional &lt; 0">
											<xsl:value-of select="$NetNotional"/>
										</xsl:when>
										<xsl:when test="$NetNotional &gt; 0">
											<xsl:value-of select="$NetNotional "/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>									
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetNotionalValue>

						<xsl:variable name="GrossNotional">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL45"/>
							</xsl:call-template>
						</xsl:variable>

						<GrossNotionalValue>
							<!--<xsl:choose>
								<xsl:when test="$Side='1'">-->
									<xsl:choose>
										<xsl:when test="$GrossNotional &gt; 0">
											<xsl:value-of select="$GrossNotional"/>
										</xsl:when>
										<xsl:when test="$GrossNotional &lt; 0">
											<xsl:value-of select="$GrossNotional * -1"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								
						</GrossNotionalValue>

						<xsl:variable name="COL37">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="number(COL37)"/>
							</xsl:call-template>
						</xsl:variable>
						
					
						<xsl:variable name="TotalCommission">
							<xsl:value-of select="number($COL37 + $COL38 + $COL39 + $COL40 + $COL41)"/>
						</xsl:variable>
						<TotalCommissionandFees>
							<xsl:choose>
								<xsl:when test="$TotalCommission &gt; 0">
									<xsl:value-of select="$TotalCommission"/>
								</xsl:when>
								<xsl:when test="$TotalCommission &lt; 0">
									<xsl:value-of select="$TotalCommission * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</TotalCommissionandFees>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>