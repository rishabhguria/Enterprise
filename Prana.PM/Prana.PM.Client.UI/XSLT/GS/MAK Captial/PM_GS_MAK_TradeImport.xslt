<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>

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

	<xsl:template name="PrevMonth">
		<xsl:param name="varPreviousMonth"/>
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
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL13"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position) and normalize-space(COL6) != 'NON-TRADE ACTIVITY' and COL8!='USD'">


					<PositionMaster>

						<xsl:variable name="varPBName">
							<xsl:value-of select="'MAK'"/>
						</xsl:variable>

						<xsl:variable name = "PB_Symbol_NAME" >
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_Symbol_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$varPBName]/SymbolData[@PBCompanyName=$PB_Symbol_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<!--   Fund -->
						<!--fundname section-->
						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL2"/>
						</xsl:variable>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$varPBName]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<!--<xsl:variable name="PRANA_STRATEGY_NAME">
				  <xsl:value-of select="document('../ReconMappingXml/StrategyMapping.xml')/StrategyMapping/PB[@Name=$varPBName]/StrategyData[@PranaFundName=$PRANA_FUND_NAME]/@PranaStrategy"/>
			  </xsl:variable>-->


						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select="COL9"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:value-of select="normalize-space(COL31)"/>
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
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="varOptionSymbol">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varEquitySymbol">
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name="varFutureSymbol">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varOptionExpiry">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varPBSymbol">
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<xsl:variable name="varDescription">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varNetPosition">
							<xsl:value-of select="COL13"/>
						</xsl:variable>

						<xsl:variable name="varCostBasis">
							<xsl:choose>
								<xsl:when test="contains(COL18,'-FOREX')">
									<xsl:value-of select="COL17 div COL13"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL14"/>		
								</xsl:otherwise>
								</xsl:choose>							
						</xsl:variable>

						<xsl:variable name="varFXConversionMethodOperator">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varFXRate">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varCommission">
							<xsl:choose>
								<xsl:when test="contains(COL6,'CFD')">
									<xsl:value-of select="''">
										
									</xsl:value-of>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL16"/>
								</xsl:otherwise>
							</xsl:choose>

						</xsl:variable>



						<xsl:variable name="varClearingFee">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varTransactionLevy">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varSecFees">
							<xsl:value-of select="0"/>
						</xsl:variable>

						<xsl:variable name="varStampDuty">
							<xsl:choose>
								<xsl:when test="string-length(COL8) = '21'">
									<xsl:value-of select="format-number(COL13*COL14*100 + COL17 - COL16,'#.00')"/>

								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number(COL13*COL14 + COL17 - COL16,'#.00')"/>															</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>


						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<FundName>
									<xsl:value-of select='$PB_FUND_NAME'/>
								</FundName>
							</xsl:when>
							<xsl:otherwise>
								<FundName>
									<xsl:value-of select='$PRANA_FUND_NAME'/>
								</FundName>
							</xsl:otherwise>
						</xsl:choose>

						<PositionStartDate>
							<xsl:value-of select="$varPositionStartDate"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:choose>
								<xsl:when test="contains(COL7,'%')">
									<xsl:value-of select="$varPositionStartDate"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="COL10"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionSettlementDate>

						<!--<Strategy>
				  <xsl:value-of select ="$PRANA_STRATEGY_NAME"/>
			  </Strategy>-->

						<xsl:choose>
							<xsl:when test="string-length(COL8) = 21">
								<Symbol>
									<xsl:value-of select="''"/>
								</Symbol>
								<IDCOOptionSymbol>
									<xsl:value-of select="concat($varOSISymbol,'U')"/>
								</IDCOOptionSymbol>
								

							</xsl:when>

							<xsl:otherwise>
								<xsl:variable name="varSuffix">
									<xsl:call-template name="GetSuffix">
										<xsl:with-param name="Suffix" select="COL5"/>
									</xsl:call-template>
								</xsl:variable>

								<Symbol>
									<xsl:choose>
										<xsl:when test="$PRANA_Symbol_NAME != ''">
											<xsl:value-of select="$PRANA_Symbol_NAME"/>
										</xsl:when>

										<xsl:when test="string-length(COL8) = 21">
											<xsl:value-of select="''"/>
										</xsl:when>
										<xsl:when test="contains(COL18,'FOREX')">
											<xsl:value-of select="concat(normalize-space(COL8),'-USD SPOT')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:choose>
												<xsl:when test ="COL4 = 'SO AFRICAN RAND' or COL4 = 'EURO' or COL4 = 'CANADIAN DOLLAR' or COL4 = 'SWISS FRANC' or COL4 = 'UK POUND STERLING'">
													<xsl:value-of select ="''"/>
												</xsl:when>
												<xsl:when test="COL4 != 'U S DOLLAR'">
													<xsl:value-of select="concat($varEquitySymbol, $varSuffix)"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$varEquitySymbol"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:otherwise>
									</xsl:choose>
								</Symbol>

								<SEDOL>
									<xsl:choose>
										<xsl:when test ="COL4 = 'SO AFRICAN RAND' or COL4 = 'EURO' or COL4 = 'CANADIAN DOLLAR' or COL4 = 'SWISS FRANC' or COL4 = 'UK POUND STERLING'">
											<xsl:value-of select ="COL8"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</SEDOL>
								<IDCOOptionSymbol>
									<xsl:value-of select="''"/>
								</IDCOOptionSymbol>
							</xsl:otherwise>
						</xsl:choose>


						<PBSymbol>
							<xsl:value-of select="$varPBSymbol"/>
						</PBSymbol>

						<!--QUANTITY-->

						<xsl:choose>
							<xsl:when test="$varNetPosition &lt; 0">
								<NetPosition>
									<xsl:value-of select="$varNetPosition * (-1)"/>
								</NetPosition>
							</xsl:when>
							<xsl:when test="$varNetPosition &gt; 0">
								<NetPosition>
									<xsl:value-of select="$varNetPosition"/>
								</NetPosition>
							</xsl:when>
							<xsl:otherwise>
								<NetPosition>
									<xsl:value-of select="0"/>
								</NetPosition>
							</xsl:otherwise>
						</xsl:choose>

						<!--Side-->

						<SideTagValue>
							<xsl:choose>

								<xsl:when test="contains(COL11,'CANCEL')">
									<xsl:choose>

										<xsl:when test="contains(COL11,'SELL TO CLOSE')">
											<xsl:value-of select="'A'"/>
										</xsl:when>

										<xsl:when test="contains(COL11,'BUY TO COVER')">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										
										<xsl:when test="contains(COL11,'CANCEL BUY')">
											<xsl:value-of select="'2'"/>
										</xsl:when>

										<!--<xsl:when test="contains(COL11,'BUY TO COVER')">
											<xsl:value-of select="'5'"/>
										</xsl:when>-->
										
										<xsl:when test="contains(COL11,'SHORT SELL')">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="contains(COL11,'CANCEL SELL')">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>

								<xsl:when test="COL6='CFD ACTIVITY'">
									<xsl:choose>


										<xsl:when test="COL11 = 'SELL TO OPEN'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'SHORT SELL'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'SELL TO CLOSE'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'BUY TO CLOSE' or COL11 = 'BUY TO COVER'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'BUY TO OPEN'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'BUY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'SELL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>


										<xsl:when test="COL11 = 'SELL TO OPEN'">
											<xsl:value-of select="'C'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'SHORT SELL'">
											<xsl:value-of select="'5'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'SELL TO CLOSE'">
											<xsl:value-of select="'D'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'BUY TO CLOSE' or COL11 = 'BUY TO COVER'">
											<xsl:value-of select="'B'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'BUY TO OPEN'">
											<xsl:value-of select="'A'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'BUY'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="COL11 = 'SELL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>

						<CostBasis>
							<!--<xsl:choose>
								<xsl:when test ="boolean(number($varCostBasis))">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>-->

							<xsl:choose>
								<xsl:when test="$varCostBasis &gt; 0">
									<xsl:value-of select="$varCostBasis"/>
								</xsl:when>
								<xsl:when test="$varCostBasis &lt; 0">
									<xsl:value-of select="$varCostBasis*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>

						<Commission>
							<xsl:choose>
								<xsl:when test="contains(COL11,'CANCEL')">
									<xsl:choose>
									<xsl:when test="$varCommission &gt; 0">
										<xsl:value-of select="$varCommission * (-1)"/>
									</xsl:when>
									<xsl:when test="$varCommission &lt; 0">
										<xsl:value-of select="$varCommission"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
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
								</xsl:otherwise>
							</xsl:choose>

						</Commission>

						<!--<Fees>
              <xsl:choose>
                <xsl:when test="$varFees &gt; 0">
                  <xsl:value-of select="$varFees"/>
                </xsl:when>
                <xsl:when test="$varFees &lt; 0">
                  <xsl:value-of select="$varFees*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </Fees>

            <MiscFees>
              <xsl:choose>
                <xsl:when test="$varMiscFees &gt; 0">
                  <xsl:value-of select="$varMiscFees"/>
                </xsl:when>
                <xsl:when test="$varMiscFees &lt; 0">
                  <xsl:value-of select="$varMiscFees*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </MiscFees>

            <ClearingFee>
              <xsl:choose>
                <xsl:when test="$varClearingFee &gt; 0">
                  <xsl:value-of select="$varClearingFee"/>
                </xsl:when>
                <xsl:when test="$varClearingFee &lt; 0">
                  <xsl:value-of select="$varClearingFee*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </ClearingFee>

            <TransactionLevy>
              <xsl:choose>
                <xsl:when test="$varTransactionLevy &gt; 0">
                  <xsl:value-of select="$varTransactionLevy"/>
                </xsl:when>
                <xsl:when test="$varTransactionLevy &lt; 0">
                  <xsl:value-of select="$varTransactionLevy*(-1)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="0"/>
                </xsl:otherwise>
              </xsl:choose>
            </TransactionLevy>

            -->

						<StampDuty>
							<xsl:choose>
								<xsl:when test="contains(COL7,'%') or contains(COL6,'CFD')">
									<xsl:value-of select="0"/>
								</xsl:when>

								<xsl:when test="contains(COL11,'CANCEL')">
									<xsl:choose>
										<xsl:when test="$varStampDuty &gt; 0">
											<xsl:value-of select="$varStampDuty * (-1)"/>
										</xsl:when>
										<xsl:when test="$varStampDuty &lt; 0">
											<xsl:value-of select="$varStampDuty"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varStampDuty &gt; 0">
											<xsl:value-of select="$varStampDuty"/>
										</xsl:when>
										<xsl:when test="$varStampDuty &lt; 0">
											<xsl:value-of select="$varStampDuty*(-1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>

						<!--<StampDuty>
							<xsl:choose>
								<xsl:when test="contains(COL7,'%') or contains(COL18,'FOREX')">
									<xsl:value-of select="0"/>
								</xsl:when>
								<xsl:when test="$varStampDuty &gt; 0">
									<xsl:value-of select="$varStampDuty"/>
								</xsl:when>
								<xsl:when test="$varStampDuty &lt; 0">
									<xsl:value-of select="$varStampDuty*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</StampDuty>-->

						<xsl:variable name="AccruedInterest" select="number(COL15)"/>

						<AccruedInterest>
							<xsl:choose>
								<xsl:when test="$AccruedInterest &gt; 0">
									<xsl:value-of select="$AccruedInterest"/>
								</xsl:when>
								<xsl:when test="$AccruedInterest &lt; 0">
									<xsl:value-of select="$AccruedInterest*(-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccruedInterest>

						<!--Swap Handling-->

						<xsl:if test="COL6='CFD ACTIVITY'">

							<IsSwapped>
								<xsl:value-of select ="1"/>
							</IsSwapped>

							<SwapDescription>
								<xsl:value-of select ="'CFD'"/>
							</SwapDescription>

							<DayCount>
								<xsl:value-of select ="365"/>
							</DayCount>

							<ResetFrequency>
								<xsl:value-of select ="'Monthly'"/>
							</ResetFrequency>

							<OrigTransDate>
								<xsl:value-of select="$varPositionStartDate"/>
							</OrigTransDate>

							<xsl:variable name="varPreviousMonth">
								<xsl:value-of select="substring-before(COL9,'/')"/>
							</xsl:variable>



							<xsl:variable name ="varPrevMonth">
								<xsl:choose>
									<xsl:when test="number($varPreviousMonth) != 1">
										<xsl:value-of select="number($varPreviousMonth) - 1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="12"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<xsl:variable name="varYearNo">
								<xsl:value-of select="substring-after(substring-after(COL9,'/'),'/')"/>
							</xsl:variable>

							<xsl:variable name ="varYear">
								<xsl:choose>
									<xsl:when test="number($varPreviousMonth) = 1">
										<xsl:value-of select="($varYearNo)-1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$varYearNo"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>

							<FirstResetDate>
								<xsl:value-of select ="concat($varPrevMonth,'/28/',$varYear)"/>
							</FirstResetDate>

						</xsl:if>
					</PositionMaster>


				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
