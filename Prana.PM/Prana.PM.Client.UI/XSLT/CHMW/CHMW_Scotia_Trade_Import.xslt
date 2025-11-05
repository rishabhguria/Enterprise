<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

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

				
				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL18"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:if test="number($Position)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'ScotiaBank'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL13"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

					

						<xsl:variable name="AssetType">
							<xsl:choose>

								<xsl:when test="contains(COL15,'OPTION')">
									<xsl:value-of select="'Option'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						
						<xsl:variable name="Symbol">
							<xsl:value-of select="substring-before(COL12,' ')"/>
						</xsl:variable>

						<Symbol>


							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="COL11!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL10!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL9!='*'">
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

								<xsl:when test="COL11!='*'">
									<xsl:value-of select="COL11"/>
								</xsl:when>
								<xsl:when test="COL10!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL9!='*'">
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


								<xsl:when test="COL11!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL10!='*'">
									<xsl:value-of select="COL10"/>
								</xsl:when>
								<xsl:when test="COL9!='*'">
									<xsl:value-of select="''"/>
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
								<xsl:when test="COL11!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL10!='*'">
									<xsl:value-of select="''"/>
								</xsl:when>
								<xsl:when test="COL9!='*'">
									<xsl:value-of select="COL9"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</ISIN>

						<Symbology>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME !=''">
									<xsl:value-of select="'Symbol'"/>
								</xsl:when>

								<xsl:when test="COL11!='*'">
									<xsl:value-of select="'CUSIP'"/>
								</xsl:when>

								<xsl:when test="COL10!='*'">
									<xsl:value-of select="'Sedol'"/>
								</xsl:when>

								<xsl:when test="COL9!='*'">
									<xsl:value-of select="'ISIN'"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="'Symbol'"/>
								</xsl:when>


								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbology>
						
						<xsl:variable name="PB_FUND_NAME" select="COL3"/>

						<xsl:variable name="PRANA_FUND_NAME">
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

						<NetPosition>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</NetPosition>

						<xsl:variable name="Side" select="COL17"/>

						<SideTagValue>
							<xsl:choose>

								<xsl:when test="COL16='CANCEL'">
									<xsl:choose>

										<xsl:when test="$Side='BL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>

										<xsl:when test="$Side='SL'">
											<xsl:value-of select="'1'"/>
										</xsl:when>

										<xsl:when test="$Side='SS'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="$Side='STO'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="$Side='BC' or $Side='CS'">
											<xsl:value-of select="'5'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>
								<xsl:when test="$AssetType='Option'">
									<xsl:choose>

										<xsl:when test="$Side='BTO'">
											<xsl:value-of select="'A'"/>
										</xsl:when>

										<xsl:when test="$Side='BL'">
											<xsl:value-of select="'A'"/>
										</xsl:when>

										<xsl:when test="$Side='SL'">
											<xsl:value-of select="'D'"/>
										</xsl:when>

										<xsl:when test="$Side='SS'">
											<xsl:value-of select="'C'"/>
										</xsl:when>

										<xsl:when test="$Side='BC'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="$Side='CS'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:when test="$Side='STO'">
											<xsl:value-of select="'C'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>

									<xsl:choose>

										<xsl:when test="$Side='BL'">
											<xsl:value-of select="'1'"/>
										</xsl:when>

										<xsl:when test="$Side='SL'">
											<xsl:value-of select="'2'"/>
										</xsl:when>

										<xsl:when test="$Side='SS'">
											<xsl:value-of select="'5'"/>
										</xsl:when>

										<xsl:when test="$Side='BC' or $Side='CS'">
											<xsl:value-of select="'B'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>

						</SideTagValue>

						<xsl:variable name="CostBasis">
							<xsl:choose>
								<xsl:when test="$AssetType='Option'">
									<xsl:value-of select="number(number(COL20) div number(COL18)) div 100"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="number(number(COL20) div number(COL18))"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test="$CostBasis &gt; 0">
									<xsl:value-of select="$CostBasis"/>
								</xsl:when>

								<xsl:when test="$CostBasis &lt; 0">
									<xsl:value-of select="$CostBasis*-1"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<xsl:variable name="Commission">
							<xsl:value-of select="number(COL21)"/>
						</xsl:variable>


						<Commission>
							<xsl:choose>
								<xsl:when test="COL16='CANCEL'">
									<xsl:choose>

										<xsl:when test="$Commission &lt; 0">
											<xsl:value-of select="$Commission"/>
										</xsl:when>

										<xsl:when test="$Commission &gt; 0">
											<xsl:value-of select="$Commission*-1"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>

									</xsl:choose>
								</xsl:when>

								<xsl:otherwise>

									<xsl:choose>

										<xsl:when test="$Commission &gt; 0">
											<xsl:value-of select="$Commission"/>
										</xsl:when>

										<xsl:when test="$Commission &lt; 0">
											<xsl:value-of select="$Commission*-1"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>

									</xsl:choose>

								</xsl:otherwise>

							</xsl:choose>

						</Commission>

						<xsl:variable name="StampDuty">
							<xsl:value-of select="number(COL24)"/>
						</xsl:variable>

						<StampDuty>
							<xsl:choose>

								<xsl:when test="$StampDuty &gt; 0">
									<xsl:value-of select="$StampDuty"/>
								</xsl:when>

								<xsl:when test="$StampDuty &lt; 0">
									<xsl:value-of select="$StampDuty*-1"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</StampDuty>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<Description>
							<xsl:value-of select="COL13"/>
						</Description>
						<xsl:variable name="Month1">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL7,' '),' ')"/>
							</xsl:call-template>
						</xsl:variable>


						<xsl:variable name="Day1">
							<xsl:value-of select="substring(substring-after(substring-after(substring-before(COL7,' '),' '),' '),1,2)"/>
						</xsl:variable>


						<xsl:variable name="Year1">
							<xsl:value-of select="substring(substring-before(COL7,' '),1,4)"/>
						</xsl:variable>


						<PositionStartDate>
							<xsl:choose>
								<xsl:when test="contains(substring-after(substring-after(normalize-space(COL7),' '),' '),'20')">
									<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL7),' '),'/',substring-after(substring-after(COL7,' '),' '))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL7),' '),'/',20,substring-after(substring-after(COL7,' '),' '))"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionStartDate>

						<xsl:variable name="Month2">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL8,' '),' ')"/>
							</xsl:call-template>
						</xsl:variable>



						<xsl:variable name="Day2">
							<xsl:value-of select="substring(substring-after(substring-after(substring-before(COL8,' '),' '),' '),1,2)"/>
						</xsl:variable>


						<xsl:variable name="Year2">
							<xsl:value-of select="substring(substring-before(COL8,' '),1,4)"/>
						</xsl:variable>


						<PositionSettlementDate>
							<xsl:choose>
								<xsl:when test="contains(substring-after(substring-after(normalize-space(COL8),' '),' '),'20')">
									<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL8),' '),'/',substring-after(substring-after(COL8,' '),' '))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL8),' '),'/',20,substring-after(substring-after(COL8,' '),' '))"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionSettlementDate>
					</PositionMaster>

				</xsl:if>
			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>
