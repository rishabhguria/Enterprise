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

	<xsl:template name="MonthCode">

		<xsl:param name="Month"/>

		<xsl:param name="PutOrCall"/>

		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month=1">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month=2">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month=3">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month=4">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month=5">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month=6">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month=7">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month=8">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month=9">
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
				<xsl:when test="$Month=1">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month=2">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month=3">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month=4">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month=5">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month=6">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month=7">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month=8">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month=9">
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
		<xsl:param name="varSymbol"/>

		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before($varSymbol,' ')"/>
		</xsl:variable>

		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring(substring-after($varSymbol,' '),1,2)"/>
		</xsl:variable>

		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring(substring-after($varSymbol,' '),5,2)"/>
		</xsl:variable>

		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring(substring-after($varSymbol,' '),3,2)"/>
		</xsl:variable>

		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring(substring-after($varSymbol,' '),7,1)"/>
		</xsl:variable>

		<xsl:variable name="StrikePriceInt">
			<xsl:value-of select="substring(substring-after($varSymbol,' '),8,5)"/>
		</xsl:variable>

		<xsl:variable name="StrikePriceDecimal">
			<xsl:value-of select="substring(substring-after($varSymbol,' '),13,3)"/>
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

		<xsl:variable name="ThirdFriday">
			<xsl:choose>
				<xsl:when test="number($ExpiryMonth) and number($ExpiryYear)">
					<xsl:value-of select="my:Now(number($ExpiryYear),number($ExpiryMonth))"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(concat($StrikePriceInt,'.',$StrikePriceDecimal),'#.00')"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="substring(substring-after($ThirdFriday,'/'),1,2) = ($ExpiryDay - 1)">
				<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>
			
			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position" select="COL18"/>

				<xsl:if test="number($Position)">
					
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'ScotiaBank'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL13"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="PB_CURRENCY" select="COL4"/>

						<xsl:variable name="PRANA_SUFFIX_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/SymbolSuffixMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBSuffixCode=$PB_CURRENCY]/@PranaSuffixCode"/>
						</xsl:variable>
						
					

						<xsl:variable name="Symbol">

							<xsl:choose>
								<xsl:when test="contains(COL12,'/')">
									<xsl:value-of select="translate(substring-before(COL12,' '),'/','.')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring-before(COL12,' ')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>					

						<xsl:variable name="AssetType" select="COL15"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$AssetType='OPTION'">
									<xsl:call-template name="Option">
										<xsl:with-param name="varSymbol" select="normalize-space(COL30)"/>
									</xsl:call-template>
								</xsl:when>

								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$PB_CURRENCY='CAD'">
											<xsl:value-of select="concat($Symbol,$PRANA_SUFFIX_NAME)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$Symbol"/>
										</xsl:otherwise>
									</xsl:choose>									
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="COL3"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
								<xsl:when test="$AssetType='OPTION'">
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
								<xsl:when test="$AssetType='OPTION'">
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
									<!--<xsl:value-of select="$Commission*-1"/>-->
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
						
						<PositionStartDate>
							<xsl:value-of select="COL7"/>
						</PositionStartDate>

					</PositionMaster>

				</xsl:if>
			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
</xsl:stylesheet>
