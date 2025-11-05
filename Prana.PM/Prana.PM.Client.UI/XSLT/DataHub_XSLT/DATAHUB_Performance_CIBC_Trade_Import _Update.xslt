<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:my="put-your-namespace-uri-here">

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


	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='01' ">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='02' ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='03' ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='04' ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='05' ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='06' ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='07'  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='08'  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='09' ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='10' ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='11' ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='12' ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='01' ">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='02' ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='03' ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='04' ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='05' ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='06' ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='07'  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='08'  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='09' ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='10' ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='11' ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='12' ">
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
		<xsl:if test="contains(COL7,'Option')">
			<xsl:variable name="UnderlyingSymbol">
				<xsl:value-of select="substring-before(COL7,' ')"/>
			</xsl:variable>
			<xsl:variable name="ExpiryMonth">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(COL7,' '),' '),' '),5,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryDay">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(COL7,' '),' '),' '),7,2)"/>
			</xsl:variable>
			<xsl:variable name="ExpiryYear">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(COL7,' '),' '),' '),3,2)"/>
			</xsl:variable>

			<xsl:variable name="PutORCall">
				<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(COL7,' '),' '),' '),' '),1,1)"/>
			</xsl:variable>
			<xsl:variable name="StrikePrice">
				<xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(substring-after(COL7,' '),' '),' '),' '),' '),'#.00')"/>
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
					<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="/">
		
		<DocumentElement>
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL8"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and COL5!='Deposit' and COL5!='Dividend'and COL5!='Withdraw'and COL5!='Expenses'and COL5!='SpotFX'">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'CIBC'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL7)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol" select="substring-before(normalize-space(COL1),'_')"/>
						<xsl:variable name="PB_SUFFIX_NAME" select="substring-after(COL1,'_')"/>
						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select="document('../../../MappingFiles/ReconMappingXml/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_SUFFIX_NAME]/@TickerSuffixCode"/>
						</xsl:variable>

						<xsl:variable name="PB_FUND_NAME" select="COL21"/>

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

						<ExecutingBroker>
							<xsl:value-of select="normalize-space(COL15)"/>
						</ExecutingBroker>


						<xsl:variable name ="AssetType">
							<xsl:choose>
								<xsl:when test="contains(substring(substring-before(substring-after(substring-after(substring-after(COL7,' '),' '),' '),' '),1,1),'P') or contains(substring(substring-before(substring-after(substring-after(substring-after(COL7,' '),' '),' '),' '),1,1),'C')">
									<xsl:value-of select="'EqyityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								
									<xsl:when test="$Symbol!=''">
									<xsl:value-of select="concat($Symbol,$PRANA_SUFFIX_NAME)"/>
				                	</xsl:when>

								<xsl:when test="$AssetType='EqyityOption'">
									<xsl:call-template name="Option">	
										<xsl:with-param name="Symbol" select="COL7"/>
										<xsl:with-param name="Suffix" select="''"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						
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

						<xsl:variable name="CostBasis" select="number(COL9)"/>

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
						
						<xsl:variable name="varCoupon">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL13"/>
							</xsl:call-template>
						</xsl:variable>
						<Coupon>
							<xsl:value-of select="$varCoupon"/>
						</Coupon>

						<Strategy>
							<xsl:value-of select="normalize-space(COL16)"/>
						</Strategy>
						<xsl:variable name="Side" select="normalize-space(COL5)"/>

						<SideTagValue>
	
									<xsl:choose>
										<xsl:when test="$Position &gt; 0">
											<xsl:value-of select="'1'"/>
										</xsl:when>

										<xsl:when test="$Position &lt; 0">
											<xsl:value-of select="'2'"/>
										</xsl:when>

										<xsl:otherwise>
											<xsl:value-of select="'0'"/>
										</xsl:otherwise>
								
							</xsl:choose>			
						</SideTagValue>

						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="Month1">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL3,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>
						<PositionStartDate>
							<xsl:choose>
								<xsl:when test="contains(substring-after(substring-after(normalize-space(COL3),'-'),'-'),'20')">
									<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL3),'-'),'/',substring-after(substring-after(COL3,'-'),'-'))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="concat($Month1,'/',substring-before(normalize-space(COL3),'-'),'/',20,substring-after(substring-after(COL3,'-'),'-'))"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionStartDate>

						<xsl:variable name="Month2">
							<xsl:call-template name="MonthName">
								<xsl:with-param name="Month" select="substring-before(substring-after(COL4,'-'),'-')"/>
							</xsl:call-template>
						</xsl:variable>

						<PositionSettlementDate>
							<xsl:choose>
								<xsl:when test="contains(substring-after(substring-after(normalize-space(COL4),'-'),'-'),'20')">
									<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL4),'-'),'/',substring-after(substring-after(COL4,'-'),'-'))"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="concat($Month2,'/',substring-before(normalize-space(COL4),'-'),'/',20,substring-after(substring-after(COL4,'-'),'-'))"/>
								</xsl:otherwise>
							</xsl:choose>
						</PositionSettlementDate>


						
						<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL11"/>
							</xsl:call-template>
						</xsl:variable>
						<Commission>
							<xsl:choose>
								<xsl:when test="COL16='Back-Out'">								
									<xsl:choose>
											<xsl:when test="$Commission &gt; 0">
												<xsl:value-of select="$Commission * (-1)"/>
											</xsl:when>
											<xsl:when test="$Commission &lt; 0">
												<xsl:value-of select="$Commission"/>
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
											<xsl:value-of select="$Commission * (-1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="0"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>																	
					</Commission>

						<xsl:variable name="StampDuty">
							<xsl:choose>
								<xsl:when test="COL5='Sell' and COL6='USD' and COL16='Back-Out'">
									<xsl:value-of select="COL12 * (-1)"/>
								</xsl:when>
								<xsl:when test="COL5='Sell' and COL6='USD'">
									<xsl:value-of select="COL12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
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

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>