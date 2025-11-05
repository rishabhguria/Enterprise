<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

	<msxsl:script language="C#" implements-prefix="my">

		public string Now(int year, int month, int date)
		{
		DateTime weekEnd= new DateTime(year, month, date);
		while (weekEnd.DayOfWeek == DayOfWeek.Saturday)
		{
		weekEnd = weekEnd.AddDays(1);
		}
		return weekEnd.ToString();
		}

		public string AddDay(int year, int month, int date)
		{
		DateTime weekEnd = new DateTime(year, month, date);
		return weekEnd.AddDays(1).ToString();
		}

	</msxsl:script>

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

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
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

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL6) and COL4!='Settled Trade'">
					<PositionMaster>

						<xsl:variable name = "PB_NAME">
							<xsl:value-of select="'Goldman Sachs'"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL8"/>
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


								<xsl:when test="COL23!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>



						<Bloomberg>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL23!='*'">
									<xsl:value-of select="COL23"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Bloomberg>


						<Symbology>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="'Symbol'"/>
								</xsl:when>

								<xsl:when test="COL23!='*'">
									<xsl:value-of select="'Bloomberg'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbology>
						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
								<xsl:with-param name="Month" select="substring-before(COL1,' ')"/>
							</xsl:call-template>
						</xsl:variable>

						<PositionStartDate>


							<xsl:value-of select="concat($Month,'/',substring-before(substring-after(COL1,' '),','),'/',substring-after(substring-after(COL1,' '),' '))"/>

						</PositionStartDate>

						<xsl:variable name="Month1">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(COL1,' ')"/>
							</xsl:call-template>
						</xsl:variable>


						<!--<xsl:variable name="varYear">
				  <xsl:value-of select="substring-after(substring-after(COL19,' '),' ')"/>
			  </xsl:variable>

			  <xsl:variable name="varMonth">
				  <xsl:value-of select="$Month1"/>
			  </xsl:variable>




			  <xsl:variable name="varDate">
				  <xsl:value-of select="substring-before(substring-after(COL19,' '),',')"/>
			  </xsl:variable>

			  <xsl:variable name="SettleDate">
				  <xsl:value-of select='my:AddDay(number($varYear),number($varMonth),number($varDate))'/>
			  </xsl:variable>

			  <xsl:variable name="Date">
				  <xsl:value-of select="COL19"/>
			  </xsl:variable>

			  <xsl:variable name="PRANA_Date_Mapping">
				  <xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/HolidayMapping.xml')/DateMapping/PB[@Name=$PB_NAME]/DateData/@Date"/>
			  </xsl:variable>

			  <xsl:variable name="DateCheck">
				  <xsl:value-of select ="concat($Month,'/',substring-before(substring-after(COL16,' '),',') + 1,'/',substring-after(substring-after(COL16,' '),' '))"/>
			  </xsl:variable>

			  <xsl:variable name="MonthMap">
				  <xsl:value-of select="substring-before($PRANA_Date_Mapping,'/')"/>
			  </xsl:variable>

			  <xsl:variable name="DayMap">
				  <xsl:choose>
					  <xsl:when test="string-length(number(substring-before(substring-after($PRANA_Date_Mapping,'/'),'/'))) = 1">
						  <xsl:value-of select="concat(0,substring-before(substring-after($PRANA_Date_Mapping,'/'),'/') + 1)"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="substring-before(substring-after($PRANA_Date_Mapping,'/'),'/') + 1"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="YearMap">
				  <xsl:value-of select="substring-after(substring-after($PRANA_Date_Mapping,'/'),'/')"/>
			  </xsl:variable>-->


						<PositionSettlementDate>

							<xsl:value-of select="concat($Month1,'/',substring-before(substring-after(COL1,' '),','),'/',substring-after(substring-after(COL1,' '),' '))"/>

							<!--<xsl:value-of select="COL20"/>-->
						</PositionSettlementDate>




						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="number(COL6)"/>
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
								<xsl:when test="COL4='Cancel'">
									<xsl:choose>
										<xsl:when test="COL5='S'">
											<xsl:value-of select="'1'"/>
										</xsl:when>
										<xsl:when test="COL5='B'">
											<xsl:value-of select="'2'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test ="COL5='SS'">
											<xsl:value-of select ="'5'"/>
										</xsl:when>

										<xsl:when test ="COL5='S'">
											<xsl:value-of select ="'2'"/>
										</xsl:when>


										<xsl:when test ="COL5='CS'">
											<xsl:value-of select ="'B'"/>
										</xsl:when>

										<xsl:when test ="COL5='B'">
											<xsl:value-of select ="'1'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select ="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>



						</SideTagValue>

						<xsl:variable name="PB_Contract" select="normalize-space(COL8)"/>

						<xsl:variable name="PRANA_Multiplier">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/MultiplierMapping.xml')/SymbolMultiplierMapping/PB[@Name=$PB_NAME]/MultiplierData[@Contract=$PB_Contract]/@PBMultiplier"/>
						</xsl:variable>

						<xsl:variable name="PRANA_MultiplierToExclude">
							<xsl:value-of select ="document('../../../MappingFiles/ReconMappingXml/MultiplierMappingToExclude.xml')/SymbolMultiplierMapping/PB[@Name=$PB_NAME]/MultiplierData[@Contract=$PB_Contract]/@PBMultiplier"/>
						</xsl:variable>

						<xsl:variable name="Price">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:variable name="CostBasis">

							<xsl:choose>
								<xsl:when test="$PRANA_MultiplierToExclude!=''">
									<xsl:value-of select="$Price"/>
								</xsl:when>
								<xsl:when test="$PRANA_Multiplier!=''">
									<xsl:value-of select="format-number($Price * $PRANA_Multiplier,'#.######')"/>
									<!--<xsl:value-of select="$Price * $PRANA_Multiplier"/>-->
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='number($Price)'/>
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
						</CostBasis>




						<!--<xsl:variable name ="varCommision">
				<xsl:call-template name="Translate">
					<xsl:with-param name="Number" select="COL16"/>
				</xsl:call-template>
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


						<!--<xsl:variable name ="OtherBrokerFees">
				<xsl:call-template name="Translate">
					<xsl:with-param name="Number" select="COL17"/>
				</xsl:call-template>
            </xsl:variable>

            <Fees>
              <xsl:choose>

                <xsl:when test ="$OtherBrokerFees &lt;0">
                  <xsl:value-of select ="$OtherBrokerFees*-1"/>
                </xsl:when>

                <xsl:when test ="$OtherBrokerFees &gt;0">
                  <xsl:value-of select ="$OtherBrokerFees"/>
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select ="0"/>
                </xsl:otherwise>

              </xsl:choose>
            </Fees>-->


						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>



						<xsl:variable name="Multiplier" select="translate(COL22,',','')"/>

						<Multiplier>

							<xsl:choose>
								<xsl:when test="$PRANA_MultiplierToExclude!=''">
									<xsl:value-of select="$PRANA_MultiplierToExclude"/>
								</xsl:when>
								<xsl:when test="$PRANA_Multiplier!=''">
									<xsl:value-of select="format-number($Multiplier div $PRANA_Multiplier,'#.##')"/>
									<!--<xsl:value-of select="$Multiplier div $PRANA_Multiplier"/>-->
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$Multiplier"/>
								</xsl:otherwise>
							</xsl:choose>

						</Multiplier>

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
